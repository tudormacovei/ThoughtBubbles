using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using tdk.Systems;

public class FrameController : Singleton<FrameController>
{
    [Header("INPUT")]
    [SerializeField] Button _left;
    [SerializeField] Button _right;

    [SerializeField]
    ItemSwitcher[] _switchers;

    [SerializeField]
    Transform[] _frames;
    SpriteRenderer[] _framefade;

    [SerializeField]
    Animator _anim;
    [SerializeField]
    SpriteRenderer _sprite;

    [SerializeField] float moveDuration;

    [SerializeField] float fadeInDuration;
    [SerializeField] float fadeOutDuration;

    public float FadeInDuration => fadeInDuration;

    int _currentFrame;

    public int CurrrentFrame => _currentFrame;

    bool _isMoving;

    public bool Moving => _isMoving;

    new void Awake()
    {
        _framefade = new SpriteRenderer[_frames.Length];

        for(int i = 0; i < _frames.Length; i++)
        {
            _framefade[i] = _frames[i].GetComponent<SpriteRenderer>();

            StartCoroutine(SpriteFade(_framefade[i], 0, 0.1f));
        }

        StartCoroutine(SpriteFade(_framefade[_currentFrame], 1, 0.1f));

        for (int i = 0; i < _switchers.Length; i++)
        {
            _switchers[i].DisableFrameFast();
        }

        _switchers[_currentFrame].EnableFrame();
    }

    public void MoveNext()
    {
        if (_currentFrame == _frames.Length - 1) return;

        int previousFrame = _currentFrame;

        if (!_isMoving)
        {
            _currentFrame++;

            _sprite.flipX = false;

            _switchers[previousFrame].DisableFrame();
            _switchers[_currentFrame].EnableFrame();

            StartCoroutine(SpriteFade(_framefade[previousFrame], 0, fadeOutDuration));
            StartCoroutine(SpriteFade(_framefade[_currentFrame], 1, fadeInDuration));

            StartCoroutine(MoveOverSeconds(transform, _frames[_currentFrame].position, moveDuration));
            StartCoroutine(BubbleManager.Instance.Move(true));
        }
    }

    public void MovePrev()
    {
        if (_currentFrame == 0) return;

        int previousFrame = _currentFrame;

        if (!_isMoving)
        {
            _currentFrame--;

            _sprite.flipX = true;

            _switchers[previousFrame].DisableFrame();
            _switchers[_currentFrame].EnableFrame();

            StartCoroutine(SpriteFade(_framefade[previousFrame], 0, fadeOutDuration));
            StartCoroutine(SpriteFade(_framefade[_currentFrame], 1, fadeInDuration));

            StartCoroutine(MoveOverSeconds(transform, _frames[_currentFrame].position, moveDuration));
            
            StartCoroutine(BubbleManager.Instance.Move(false)); // move bubbles along
        }
    }

    IEnumerator MoveOverSeconds(Transform objectToMove, Vector3 end, float seconds)
    {
        DisableButtons();

        _anim.SetBool("IsWalking", true);

        _isMoving = true;
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        while (elapsedTime < seconds)
        {
            objectToMove.transform.position = Vector3.Lerp(startingPos, end, elapsedTime / seconds);
            objectToMove.transform.position = new Vector3(objectToMove.transform.position.x, objectToMove.transform.position.y, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        objectToMove.transform.position = end;
        objectToMove.transform.position = new Vector3(objectToMove.transform.position.x, objectToMove.transform.position.y, 0);
        _isMoving = false;

        _anim.SetBool("IsWalking", false);

        EnableButtons();
    }

    public static IEnumerator SpriteFade(SpriteRenderer sr, float endValue, float duration)
    {
        float elapsedTime = 0;
        float startValue = sr.color.a;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, newAlpha);
            yield return null;
        }
    }

    void DisableButtons()
    {
        _right.gameObject.SetActive(false);
        _left.gameObject.SetActive(false);
    }

    void EnableButtons()
    {
        if (_currentFrame == 0)
        {
            _right.gameObject.SetActive(true);
            _left.gameObject.SetActive(false);

            return;
        }

        if (_currentFrame == _frames.Length - 1)
        {
            _left.gameObject.SetActive(true);
            _right.gameObject.SetActive(false);

            return;
        }

        _left.gameObject.SetActive(true);
        _right.gameObject.SetActive(true);
    }
}
