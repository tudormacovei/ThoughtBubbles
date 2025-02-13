using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PopInAtSpawn : MonoBehaviour
{
    [SerializeField] float _animDuration;
    [SerializeField] float _animDurationOut;
    [SerializeField] List<CircleCollider2D> _colliders;
    [SerializeField] GameObject _parentBone;

    float _endColliderSize;
    float _startColliderSize;
    bool _disabledExternalCollider;
    bool _isPopping;
    float _lifetime;
    Vector3 _startScale;
    Vector3 _endScale;

    // the value of this function is a bounce effect, as the variable x goes from 0 to 1
    public static float BounceEffect(float x)
    {
        float a = Mathf.Sin(3.0f * x);
        float b = Mathf.Sin(5.0f * x - 3.0f) + 0.09f;
        return Mathf.Max(a, b);
    }

    public static float BounceOutEffect(float x)
    {
        float a = Mathf.Sin(1.2f * x + 1.5f);
        float b = Mathf.Pow(x, 3.0f) - 0.1f;
        return Mathf.Max(a, b);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var collider in _colliders)
        {
            collider.enabled = false;
        }
        _disabledExternalCollider = false;

        _lifetime = 0.0f;
        _endScale = _parentBone.transform.localScale;
        _startScale = new Vector3(0.1f, 0.1f, 0.1f);
        _parentBone.transform.localScale = _startScale;

        _endColliderSize = GetComponent<CircleCollider2D>().radius;
        _startColliderSize = 0.01f;
        GetComponent<CircleCollider2D>().radius = _startColliderSize;
        _isPopping = false;

        // this small fuzzyness in scale acts as a random seed for the sprite material
        _parentBone.transform.parent.GameObject().transform.localScale = Vector3.one * Random.Range(0.99f, 1.01f); 
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPopping)
        {
            _lifetime += Time.deltaTime * (1.0f / _animDurationOut);
        }
        else
        {
            _lifetime += Time.deltaTime * (1.0f / _animDuration);
        }
        if (_lifetime <= 1)
        {
            if (_isPopping)
            {
                Color color = _parentBone.transform.parent.gameObject.GetComponent<SpriteRenderer>().color;
                color.a = Mathf.Clamp(1.0f - Mathf.Pow(_lifetime, 8.0f), 0.0f, 1.0f);
                _parentBone.transform.parent.gameObject.GetComponent<SpriteRenderer>().color = color;
                _parentBone.transform.localScale = Vector3.LerpUnclamped(_startScale, _endScale, BounceOutEffect(_lifetime));
            }
            else
            {
                _parentBone.transform.localScale = Vector3.LerpUnclamped(_startScale, _endScale, BounceEffect(_lifetime));
            }
            GetComponent<CircleCollider2D>().radius = Mathf.Lerp(_startColliderSize, _endColliderSize, _lifetime);
        }
        else if (!_disabledExternalCollider)
        {
            GetComponent<CircleCollider2D>().enabled = false;
            foreach (var collider in _colliders)
            {
                collider.enabled = true;
            }
            _disabledExternalCollider = true;
            Debug.Log("Disabled external collider");
        }
        else if (_isPopping)
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }

    public void PopOut()
    {
        foreach (var collider in _colliders)
        {
            collider.enabled = false;
        }
        _startScale = new Vector3(0.0f, 0.0f, 0.0f);
        _endScale = _parentBone.transform.localScale;
        _startColliderSize = 0.3f;
        _endColliderSize = GetComponent<CircleCollider2D>().radius * 1.2f; // exaggerate effect
        _lifetime = 0.0f;
        _isPopping = true;
    }
}
