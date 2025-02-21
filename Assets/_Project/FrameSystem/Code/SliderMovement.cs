using UnityEngine;

public class SliderMovement : MonoBehaviour
{
    [SerializeField] GameObject _sliderIndicator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Alpha must be a value between 0 and 1
    public void SetTimeOfDay(float alpha)
    {
        Vector3 startPosition = new(-7.3f, 0.0f, 2.0f);
        Vector3 endPosition = new(7.3f, 0.0f, 2.0f);

        _sliderIndicator.transform.localPosition = Vector3.Lerp(startPosition, endPosition, alpha);
    }
}
