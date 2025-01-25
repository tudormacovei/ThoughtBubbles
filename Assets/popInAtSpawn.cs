using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class popInAtSpawn : MonoBehaviour
{
    public float AnimDuration;
    public float AnimDurationOut;
    public List<CircleCollider2D> Colliders;
    public GameObject ParentBone;

    float endColliderSize;
    float startColliderSize;
    bool disabledExternalCollider;
    bool isPopping;
    float lifetime;
    Vector3 startScale;
    Vector3 endScale;

    // the value of this function is a bounce effect, as the variable x goes from 0 to 1
    float BounceEffect(float x)
    {
        float a = Mathf.Sin(3.0f * x);
        float b = Mathf.Sin(5.0f * x - 3.0f) + 0.09f;
        return Mathf.Max(a, b);
    }

    float BounceOutEffect(float x)
    {
        float a = Mathf.Sin(1.2f * x + 1.5f);
        float b = Mathf.Pow(x, 3.0f) - 0.1f;
        return Mathf.Max(a, b);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var collider in Colliders)
        {
            collider.enabled = false;
        }
        disabledExternalCollider = false;

        lifetime = 0.0f;
        endScale = ParentBone.transform.localScale;
        startScale = new Vector3(0.1f, 0.1f, 0.1f);
        ParentBone.transform.localScale = startScale;

        endColliderSize = GetComponent<CircleCollider2D>().radius;
        startColliderSize = 0.01f;
        GetComponent<CircleCollider2D>().radius = startColliderSize;
        isPopping = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPopping)
        {
            lifetime += Time.deltaTime * (1.0f / AnimDurationOut);
        }
        else
        {
            lifetime += Time.deltaTime * (1.0f / AnimDuration);
        }
        if (lifetime <= 1)
        {
            if (isPopping)
            {
                Color color = ParentBone.transform.parent.gameObject.GetComponent<SpriteRenderer>().color;
                color.a = Mathf.Clamp(1.0f - Mathf.Pow(lifetime, 8.0f), 0.0f, 1.0f);
                ParentBone.transform.parent.gameObject.GetComponent<SpriteRenderer>().color = color;
                ParentBone.transform.localScale = Vector3.LerpUnclamped(startScale, endScale, BounceOutEffect(lifetime));
            }
            else
            {
                ParentBone.transform.localScale = Vector3.LerpUnclamped(startScale, endScale, BounceEffect(lifetime));
            }
            GetComponent<CircleCollider2D>().radius = Mathf.Lerp(startColliderSize, endColliderSize, lifetime);
        }
        else if (!disabledExternalCollider)
        {
            GetComponent<CircleCollider2D>().enabled = false;
            foreach (var collider in Colliders)
            {
                collider.enabled = true;
            }
            disabledExternalCollider = true;
            Debug.Log("Disabled external collider");
        }
        else if (isPopping)
        {
            Destroy(gameObject.transform.parent.gameObject);
        }

        // Only for testing
        if (lifetime > 50)
        {
            PopOut();
        }
    }

    public void PopOut()
    {
        foreach (var collider in Colliders)
        {
            collider.enabled = false;
        }
        startScale = new Vector3(0.0f, 0.0f, 0.0f);
        endScale = ParentBone.transform.localScale;
        startColliderSize = 0.3f;
        endColliderSize = GetComponent<CircleCollider2D>().radius * 1.2f; // exaggerate effect
        lifetime = 0.0f;
        isPopping = true;
    }
}
