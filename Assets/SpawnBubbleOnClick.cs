using UnityEngine;

public class SpawnBubbleOnClick : MonoBehaviour
{
    public GameObject spawnObject; // object to spawn on click

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 offset = new Vector3(0, 0, 10);
            Instantiate(spawnObject, pos + offset, Quaternion.identity);
        }
    }
}
