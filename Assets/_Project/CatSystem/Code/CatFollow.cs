using UnityEngine;

public class CatFollow : MonoBehaviour
{
	[SerializeField] Transform target = null;
	[SerializeField] float _trackSpeed = 2.0f;
	[SerializeField] float _innerBuffer = 0.1f;
	[SerializeField] float _outerBuffer = 1.5f;

	[SerializeField] SpriteRenderer _spriteRenderer;
	[SerializeField] Animator _animator;

	[SerializeField] Vector3 _offset;

	[SerializeField] float _leftXOffset = -1;
	[SerializeField] float _rightXOffset = 1;

    Vector3 _offsetHandle;

    bool _isFollowing;
	bool IsFollowing 
	{
        get
        {
			return _isFollowing;
        }
		set
        {
			_isFollowing = value;
			_animator.SetBool("IsWalking", value);

			_spriteRenderer.flipX = FrameController.Instance.GetComponentInChildren<SpriteRenderer>().flipX;

			if (FrameController.Instance.CurrrentFrame == 0)
            {
				_offsetHandle = new Vector3(_leftXOffset, 0, 0) + _offset;

				if (!value)
					_spriteRenderer.flipX = false;

				return;
            }

			if (FrameController.Instance.CurrrentFrame == 3)
            {
				_offsetHandle = new Vector3(_rightXOffset, 0, 0) + _offset;

				if (!value)
					_spriteRenderer.flipX = true;

				return;
			}
        } 
	}

	void Awake()
	{
		target = FrameController.Instance.transform;
	}

	void Update()
	{
		Vector3 cameraTargetPosition = target.position + _offsetHandle;
		Vector3 heading = cameraTargetPosition - transform.position;
		float distance = heading.magnitude;
		Vector3 direction = heading / distance;

		if (distance > _outerBuffer)
			IsFollowing = true;

		if (IsFollowing)
		{
			if (distance > _innerBuffer)
				transform.position += direction * Time.deltaTime * _trackSpeed * Mathf.Max(distance, 1f);
			else
			{
				transform.position = cameraTargetPosition;
				IsFollowing = false;
			}
		}
	}
}
