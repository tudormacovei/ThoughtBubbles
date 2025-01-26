using System.Collections.Generic;
using Unity.Hierarchy;
using UnityEngine;

public class BubbleMaterialSetup : MonoBehaviour
{
    public List<float> frameCount;
    public List<Texture2DArray> tex;

    [SerializeField, Range(0, 3)]
    public int AnimationIndex;

    public bool RandomizeAnimation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (RandomizeAnimation)
        {
            SetAnimation(Random.Range(0, 4));
        }
        else
        {
            SetAnimation(AnimationIndex);
        }
    }

    public void SetAnimation(int idx)
    {
        MaterialPropertyBlock properties = new();
        GetComponent<Renderer>().GetPropertyBlock(properties);

        // Set random values
        properties.SetFloat("_Frames", frameCount[idx]);
        properties.SetFloat("_StartTime", Random.Range(0.0f, 4.0f));
        properties.SetTexture("_Anim", tex[idx]);

        // Apply the property block to the renderer
        GetComponent<Renderer>().SetPropertyBlock(properties);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
