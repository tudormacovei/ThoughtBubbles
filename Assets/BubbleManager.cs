using JetBrains.Annotations;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class BubbleManager : MonoBehaviour
{
    public GameObject BubbleClass; // Class from which to spawn bubbles
    public static BubbleManager Instance { get; private set; } // Singleton pattern

    private List<GameObject> BubbleList; // all spawned bubbles

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Spawns a bubble with the specified world-space location, with a random rotation
    public void AddBubble(Vector3 position)
    {
        var from = Random.onUnitSphere;
        var to = Random.onUnitSphere;
        from.z = 0.0f;
        to.z = 0.0f;
        Quaternion rotation = Quaternion.FromToRotation(from, to);
        Vector3 offset = new Vector3(0, 0, 10); // to ensure the sprite is visible
        var obj = Instantiate(BubbleClass, position + offset, rotation);
        
        BubbleList.Add(obj);
    }

    public int RandomBubbleIndex()
    {
        return Mathf.RoundToInt(Random.Range(0.0f, (float)BubbleList.Count));
    }

    // remove bubble at a specific index
    public void RemoveBubble(int index)
    {
        BubbleList[index].GetComponent<popInAtSpawn>().PopOut();
        BubbleList.RemoveAt(index);
    }

    // removes a bubble from a random location
    public void RemoveBubble()
    {
        RemoveBubble(RandomBubbleIndex());
    }

    public Transform TransformAtIndex(int index)
    {
        return BubbleList[index].transform;
    }
}
