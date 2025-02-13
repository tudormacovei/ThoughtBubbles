using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PopInPopOutCollider : MonoBehaviour
{
    [SerializeField] float _animDuration;
    [SerializeField] CapsuleCollider2D _collider;

    Vector2 _endColliderSize;
    Vector2 _startColliderSize;
    
    bool _isPopping;
    float _lifetime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _lifetime = 2.0f;
        _endColliderSize = _collider.size;
        _startColliderSize = new Vector2(0.1f, 0.1f);
        
        _isPopping = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPopping)
        {
            _lifetime += Time.deltaTime * (1.0f / _animDuration);
        }

        if (_lifetime <= 1)
        {
            _collider.size = Vector2.LerpUnclamped(_startColliderSize, _endColliderSize, PopInAtSpawn.BounceEffect(_lifetime));
        }
        else
        {
            _isPopping = false;
        }
    }

    public void StartPopping()
    {
        _lifetime = 0.0f;
        _isPopping = true;
        _collider.size = _startColliderSize;
    }
}
