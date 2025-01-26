using Unity.Hierarchy;
using UnityEngine;

public class BubbleMaterialSetup : MonoBehaviour
{
    public Renderer spriteRenderer;
    public float val;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MaterialPropertyBlock properties = new();
        GetComponent<Renderer>().GetPropertyBlock(properties);

        // Set random values
        properties.SetFloat("_StartTime", Random.Range(0.0f, 4.0f));
        properties.SetFloat("_TimeMulti", Random.Range(0.6f, 1.0f));

        // Apply the property block to the renderer
        GetComponent<Renderer>().SetPropertyBlock(properties);
    }

    // Update is called once per frame
    void Update()
    {
        //MaterialPropertyBlock properties = new();
        //GetComponent<Renderer>().GetPropertyBlock(properties);

        //// Set random values
        //properties.SetFloat("_StartTime", Random.Range(0.0f, 4.0f));
        //properties.SetFloat("_TimeMulti", Random.Range(0.6f, 1.0f));

        //// Apply the property block to the renderer
        //GetComponent<Renderer>().SetPropertyBlock(properties);
    }
}
