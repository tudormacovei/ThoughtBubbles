using UnityEngine;

public class OutwardsForce : MonoBehaviour
{
    public GameObject Midpoint;


    public float strength = 100.0f;
    public float applyRadius = 0.4f; // force is applied if object is applyRadius distance away from the center of its parent (or less)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //if ((Midpoint.transform.localPosition - this.transform.localPosition).magnitude < applyRadius)
        //{
        //    float multiplier = (applyRadius - (Midpoint.transform.localPosition - this.transform.localPosition).magnitude) / applyRadius; // between 0 and 1
        //    // multiplier *= multiplier;
        //    Vector3 force = (Midpoint.transform.localPosition - this.transform.localPosition).normalized * strength * multiplier;
        //    this.GetComponent<Rigidbody2D>().AddForce(force);
        //    Midpoint.GetComponent<Rigidbody2D>().AddForce(-force);
        //    Debug.Log(multiplier);
        //}
    }
}
