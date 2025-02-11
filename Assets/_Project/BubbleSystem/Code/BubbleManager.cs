using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class BubbleManager : MonoBehaviour
{
    public GameObject BubbleClass; // Class from which to spawn bubbles
    public List<GameObject> SpawnPositions;
    public static BubbleManager Instance { get; private set; } // Singleton pattern

    private List<GameObject> BubbleList; // all spawned bubbles
    private int BubbleCount; // tracks the number of bubbles that still exist in the level, both spawned or unspawned
    public float SpawnDelay;

    [SerializeField]
    PlayCutscene _endScene;

    public bool IsSpawning;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BubbleList = new List<GameObject>();
        BubbleCount = 0;
        SpawnPositions = SpawnPositions.OrderBy(go => -go.transform.position.y).ToList(); // sort by y-position, negative so the values with the highest y are first
    }

    // Update is called once per frame
    void Update()
    {
        TimeSinceCatPop += Time.deltaTime;
    }

    IEnumerator SpawnBubblesRoutine()
    {
        if (BubbleCount == 0)
        {
            yield break;
        }
        while (enabled)
        {
            IsSpawning = true;
            Vector3 position = SpawnPositions[BubbleList.Count % SpawnPositions.Count].transform.position;
            AddBubble(position);
            BubbleCount--; // this is modified by the line above, we don't want that

            if (BubbleList.Count == BubbleCount)
            {
                Debug.Log("Stopping coroutine...");
                IsSpawning = false;
                StopAllCoroutines();
                yield break;
            }
            yield return new WaitForSeconds(Random.Range(0.0f + SpawnDelay, 0.1f + SpawnDelay));
        }
    }

    // Spawns a bubble with the specified world-space location, with a random rotation
    public void AddBubble(Vector3 position)
    {
        // if there are more bubbles spawned than can fit on the screen, end game
        if (BubbleCount >= SpawnPositions.Count)
        {
            _endScene.transform.GetChild(0).position = FrameController.Instance.transform.position;
            _endScene.PlayVideo();
            return;
        }

        var from = Random.onUnitSphere;
        var to = Random.onUnitSphere;
        from.z = 0.0f;
        to.z = 0.0f;
        Quaternion rotation = Quaternion.FromToRotation(from, to);
        var obj = Instantiate(BubbleClass, position, rotation);
        obj.transform.localScale *= Random.Range(0.65f, 1.0f);
        
        BubbleCount++;
        BubbleList.Add(obj);
    }

    public int RandomBubbleIndex()
    {
        return Mathf.RoundToInt(Random.Range(0.0f, (float)BubbleList.Count - 1.0f));
    }

    // remove bubble at a specific index
    public void RemoveBubble(int index)
    {
        BubbleList[index].GetComponentInChildren<popInAtSpawn>().PopOut();
        BubbleList.RemoveAt(index);
        BubbleCount--;
    }

    // removes a bubble from a random location
    public void RemoveBubble()
    {
        if (BubbleCount <= 0)
        {
            return;
        }
        RemoveBubble(RandomBubbleIndex());
    }

    // precondition: index must be between 0 and BubbleList.Count
    public Transform TransformAtIndex(int index)
    {
        return BubbleList[index].transform;
    }

    // Despawn bubbles, but maintain the current amount of spawned bubbles in the BubbleCount Variable
    // ! This function needs 0.4 seconds to safely complete
    public void RemoveAllBubbles()
    {
        BubbleCount += BubbleCount; // the operation below will subtract the current number of existing bubbles
        for (int i = 0; i < BubbleList.Count; i++)
        {
            Invoke(nameof(RemoveBubble), Random.Range(0.1f, 0.4f));
        }
        // the invoke above modifies BubbleCount, so we keep the initial value
    }

    // Re-Add bubbles that existed before, based on BubbleCount
    public void AddExistingBubbles()
    {
        if (BubbleList.Count != 0)
       {
           Debug.Log("Attempted re-instantiation of bubbles with nonempty list. Aborting!");
           return;
       }

       Debug.Log(BubbleCount);

       StartCoroutine(SpawnBubblesRoutine());
    }

    public IEnumerator Move(bool moveRight)
    {
        RemoveAllBubbles();
        yield return new WaitForSeconds(0.7f);

        DialogManager.Instance.Move(moveRight);
        Vector3 offset = new Vector3(4.0f, 0.0f, 0.0f);
        if (moveRight)
        {
            transform.position += offset;
        }
        else
        {
            transform .position -= offset;
        }
        AddExistingBubbles();
    }

    public void HandleDamage(int amount)
    {
        // this function exists to bind the coroutine below to this object
        StartCoroutine(HandleDamageAsync(amount));
    }

    public IEnumerator HandleDamageAsync(int amount)
    {
        IsSpawning = true;

        FrameController.Instance.DisableButtons();

        while (enabled)
        {
            Debug.Log("Amount of damage: " + amount.ToString());
            Vector3 position = SpawnPositions[BubbleList.Count % SpawnPositions.Count].transform.position;
            if (amount == 0)
            {
                FrameController.Instance.EnableButtons();

                IsSpawning = false;
                StopAllCoroutines();
                yield break;
            }
            else if (amount > 0)
            {
                AddBubble(position);
                amount--;
            }
            else
            {
                if (BubbleCount <= 0)
                {
                    yield break;
                }
                RemoveBubble();
                amount++;
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    [SerializeField] private float CatPopCooldown;
    private float TimeSinceCatPop = 0.0f;

    public void PopClick(int index)
    {
        if (TimeSinceCatPop >= CatPopCooldown)
        {
            return;
        }
        RemoveBubble(index);
        TimeSinceCatPop = 0.0f;
    }
}
