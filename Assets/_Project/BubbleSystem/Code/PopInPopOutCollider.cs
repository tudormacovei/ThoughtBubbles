using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PopInPopOutCollider : MonoBehaviour
{
    public float AnimDuration;

    public CapsuleCollider2D Collider;

    Vector2 endColliderSize;
    Vector2 startColliderSize;
    
    bool isPopping;
    float lifetime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lifetime = 2.0f;
        endColliderSize = Collider.size;
        startColliderSize = new Vector2(0.1f, 0.1f);
        
        isPopping = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPopping)
        {
            lifetime += Time.deltaTime * (1.0f / AnimDuration);
        }

        if (lifetime <= 1)
        {
            Collider.size = Vector2.LerpUnclamped(startColliderSize, endColliderSize, popInAtSpawn.BounceEffect(lifetime));
        }
        else
        {
            isPopping = false;
        }
    }

    public void StartPopping()
    {
        lifetime = 0.0f;
        isPopping = true;
        Collider.size = startColliderSize;
    }
}
