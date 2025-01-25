using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class popInAtSpawn : MonoBehaviour
{
    public float AnimDuration;
    public List<CircleCollider2D> Colliders;
    public GameObject Parent;

    float endColliderSize;
    float startColliderSize;
    bool disabledExternalCollider;
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
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var collider in Colliders)
        {
            collider.enabled = false;
        }
        disabledExternalCollider = false;

        lifetime = 0.0f;
        endScale = Parent.transform.localScale;
        startScale = new Vector3(0.1f, 0.1f, 0.1f);
        Parent.transform.localScale = startScale;

        endColliderSize = GetComponent<CircleCollider2D>().radius;
        startColliderSize = 0.01f;
        GetComponent<CircleCollider2D>().radius = startColliderSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (lifetime <= 1)
        {
            lifetime += Time.deltaTime * (1.0f / AnimDuration);
            
            Parent.transform.localScale = Vector3.LerpUnclamped(startScale, endScale, BounceEffect(lifetime));
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
    }
}
