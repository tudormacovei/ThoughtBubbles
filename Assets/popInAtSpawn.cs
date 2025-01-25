using Unity.VisualScripting;
using UnityEngine;

public class popInAtSpawn : MonoBehaviour
{
    float lifetime;
    public float animDuration;
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
        lifetime = 0.0f;
        endScale = transform.localScale;
        startScale = new Vector3(0.1f, 0.1f, 0.1f);
        transform.localScale = startScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (lifetime <= 1)
        {
            lifetime += Time.deltaTime * (1.0f / animDuration);
            transform.localScale = Vector3.LerpUnclamped(startScale, endScale, BounceEffect(lifetime));
        }
    }
}
