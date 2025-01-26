using UnityEngine;

public class CatFollow : MonoBehaviour
{
	[SerializeField] private Transform target = null;
	[SerializeField] private float _trackSpeed = 2.0f;
	[SerializeField] private float _innerBuffer = 0.1f;
	[SerializeField] private float _outerBuffer = 1.5f;

	[SerializeField] private SpriteRenderer _rend;
	[SerializeField] private Animator _animator;

	[SerializeField] Vector3 _offsetHandle;
	[SerializeField] float LeftXOffset = -1;
	[SerializeField] float RightXOffset = 1;

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

			_rend.flipX = FrameController.Instance.GetComponentInChildren<SpriteRenderer>().flipX;

			if (FrameController.Instance.CurrrentFrame == 0)
            {
				_offsetHandle = new Vector3(LeftXOffset, 0, 0);

				if (!value)
					_rend.flipX = false;

				return;
            }

			if (FrameController.Instance.CurrrentFrame == 3)
            {
				_offsetHandle = new Vector3(RightXOffset, 0, 0);

				if (!value)
					_rend.flipX = true;

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
