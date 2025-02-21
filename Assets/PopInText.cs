using TMPro;
using UnityEngine;

public class PopInText : MonoBehaviour
{
    [SerializeField] float _popDuration = 1.5f;
    [SerializeField] float _fadeoutDuration = 3.0f;
    [SerializeField] float _floatUpSpeed = 10.0f;

    float _elapsedTime;
    Vector3 _startingScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _elapsedTime = 0.0f;
        _startingScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime < _popDuration)
        {
            float popAlpha = _elapsedTime / _popDuration;
            float scale = PopInAtSpawn.BounceEffect(popAlpha);
            transform.localScale = _startingScale * scale;
        }

        var colorAlpha = (_elapsedTime / _fadeoutDuration);
        if (colorAlpha <= 1.0f)
        {
            GetComponent<TextMeshProUGUI>().alpha = 1.1f - Mathf.Pow(colorAlpha, 6.0f);
        }

        Vector3 positionDelta = new(0.0f, _floatUpSpeed * Time.deltaTime, 0.0f);
        transform.localPosition = transform.localPosition + positionDelta;
        
        if (_elapsedTime > _fadeoutDuration + 0.5f)
        {
            Destroy(gameObject);
        }
    }
}
