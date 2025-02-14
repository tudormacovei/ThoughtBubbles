using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using static UnityEditor.PlayerSettings;

public class BubbleManager : MonoBehaviour
{
    public static BubbleManager Instance { get; private set; } // Singleton pattern

    [SerializeField] GameObject _bubbleClass; // Class from which to spawn bubbles
    [SerializeField] List<GameObject> _spawnPositions;
    [SerializeField] float _spawnDelay;

    [SerializeField] float _catPopCooldown;

    List<GameObject> _bubbleList; // all spawned bubbles
    int _bubbleCount; // tracks the number of bubbles that still exist in the level, both spawned or unspawned
    
    [SerializeField] PlayCutscene _endScene;

    float _timeSinceCatPop = 0.0f;

    public bool IsSpawning { get; private set; }

    private void Awake()
    {
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
        _bubbleList = new List<GameObject>();
        _bubbleCount = 0;
        
        // sort spawn positions by y-position, negative so the values with the highest y are first
        _spawnPositions = _spawnPositions.OrderBy(go => -go.transform.position.y).ToList();
    }

    // Update is called once per frame
    void Update()
    {
        _timeSinceCatPop += Time.deltaTime;
    }

    IEnumerator SpawnBubblesRoutine()
    {
        if (_bubbleCount == 0)
        {
            yield break;
        }
        while (enabled)
        {
            IsSpawning = true;
            Vector3 position = _spawnPositions[_bubbleList.Count % _spawnPositions.Count].transform.position;
            AddBubble(position);
            _bubbleCount--; // this is modified by the line above, we don't want that

            if (_bubbleList.Count == _bubbleCount)
            {
                Debug.Log("Stopping coroutine...");
                IsSpawning = false;
                StopAllCoroutines();
                yield break;
            }
            yield return new WaitForSeconds(Random.Range(0.0f + _spawnDelay, 0.1f + _spawnDelay));
        }
    }

    // Spawns a bubble with the specified world-space location, with a random rotation
    public void AddBubble(Vector3 position)
    {
        // if there are more bubbles spawned than can fit on the screen, END GAME
        if (_bubbleCount >= _spawnPositions.Count)
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
        var obj = Instantiate(_bubbleClass, position, rotation);
        obj.transform.localScale *= Random.Range(0.65f, 1.0f);
        
        _bubbleCount++;
        _bubbleList.Add(obj);
    }

    // Precondition: 0 <= index < BubbleCount
    public Vector3 GetBubblePosition(int index)
    {
        foreach (Transform transform in _bubbleList[index].transform)
        {
            if (transform.CompareTag("BubbleCenter"))
            {
                return transform.position;
            }
        }
        Debug.Log("Could not find bubble transform!");
        return Vector3.zero;
    }

    public int RandomBubbleIndex()
    {
        return Mathf.RoundToInt(Random.Range(0.0f, (float)_bubbleList.Count - 1.0f));
    }

    // remove bubble at a specific index
    public void RemoveBubble(int index)
    {
        _bubbleList[index].GetComponentInChildren<PopInAtSpawn>().PopOut();
        _bubbleList.RemoveAt(index);
        _bubbleCount--;
    }

    // removes a bubble from a random location
    public void RemoveBubble()
    {
        if (_bubbleCount <= 0)
        {
            return;
        }
        RemoveBubble(RandomBubbleIndex());
    }

    // precondition: index must be between 0 and BubbleList.Count
    public Transform TransformAtIndex(int index)
    {
        return _bubbleList[index].transform;
    }

    // Despawn bubbles, but maintain the current amount of spawned bubbles in the BubbleCount Variable
    // ! This function needs 0.4 seconds to safely complete
    public void RemoveAllBubbles()
    {
        _bubbleCount += _bubbleCount; // the operation below will subtract the current number of existing bubbles
        for (int i = 0; i < _bubbleList.Count; i++)
        {
            Invoke(nameof(RemoveBubble), Random.Range(0.1f, 0.4f));
        }
        // the invoke above modifies BubbleCount, so we keep the initial value
    }

    // Re-Add bubbles that existed before, based on BubbleCount
    public void AddExistingBubbles()
    {
        if (_bubbleList.Count != 0)
       {
           Debug.Log("Attempted re-instantiation of bubbles with nonempty list. Aborting!");
           return;
       }

       Debug.Log(_bubbleCount);

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
            Debug.Log("Amount of damage left to deal: " + amount.ToString());
            Vector3 position = _spawnPositions[_bubbleList.Count % _spawnPositions.Count].transform.position;
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
                if (_bubbleCount <= 0)
                {
                    FrameController.Instance.EnableButtons();

                    IsSpawning = false;
                    StopAllCoroutines();
                    yield break;
                }
                RemoveBubble();
                amount++;
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    public void PopClick(int index)
    {
        if (_timeSinceCatPop >= _catPopCooldown)
        {
            return;
        }
        RemoveBubble(index);
        _timeSinceCatPop = 0.0f;
    }
    
    public int GetBubbleCount()
    {
        return _bubbleCount;
    }
}
