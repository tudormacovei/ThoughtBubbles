using System.Collections.Generic;
using Unity.Hierarchy;
using UnityEngine;

public class BubbleMaterialSetup : MonoBehaviour
{
    [SerializeField] List<float> _frameCount;
    [SerializeField] List<Texture2DArray> _tex;
    [SerializeField] List<Texture2DArray> texInterior;

    [SerializeField, Range(0, 3)]
    int _animationIndex;

    [SerializeField] bool _randomizeAnimation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_randomizeAnimation)
        {
            SetAnimation(Random.Range(0, 4));
        }
        else
        {
            SetAnimation(_animationIndex);
        }
    }

    public void SetAnimation(int idx)
    {
        MaterialPropertyBlock properties = new();
        GetComponent<Renderer>().GetPropertyBlock(properties);

        // Set random values
        properties.SetFloat("_Frames", _frameCount[idx]);
        properties.SetFloat("_StartTime", Random.Range(0.0f, 4.0f));
        properties.SetTexture("_Anim", _tex[idx]);
        properties.SetTexture("_AnimInt", texInterior[idx]);

        // Apply the property block to the renderer
        GetComponent<Renderer>().SetPropertyBlock(properties);
    }
}
