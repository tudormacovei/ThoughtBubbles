using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class FrameController : MonoBehaviour
{
    [Header("INPUT")]
    [SerializeField] Button _left;
    [SerializeField] Button _right;

    [SerializeField]
    Transform[] _frames;
    SpriteRenderer[] _framefade;

    [SerializeField] float moveDuration;

    [SerializeField] float fadeInDuration;
    [SerializeField] float fadeOutDuration;

    int _currentFrame;

    bool _isMoving;

    void Awake()
    {
        _framefade = new SpriteRenderer[_frames.Length];

        for(int i = 0; i < _frames.Length; i++)
        {
            _framefade[i] = _frames[i].GetComponent<SpriteRenderer>();

            StartCoroutine(SpriteFade(_framefade[i], 0, 0.1f));
        }

        StartCoroutine(SpriteFade(_framefade[_currentFrame], 1, 0.1f));
    }

    public void MoveNext()
    {
        if (_currentFrame == _frames.Length - 1) return;

        int previousFrame = _currentFrame;

        if (!_isMoving)
        {
            _currentFrame++;

            StartCoroutine(SpriteFade(_framefade[previousFrame], 0, fadeOutDuration));
            StartCoroutine(SpriteFade(_framefade[_currentFrame], 1, fadeInDuration));

            StartCoroutine(MoveOverSeconds(transform, _frames[_currentFrame].position, moveDuration));
        }
    }

    public void MovePrev()
    {
        if (_currentFrame == 0) return;

        int previousFrame = _currentFrame;

        if (!_isMoving)
        {
            _currentFrame--;

            StartCoroutine(SpriteFade(_framefade[previousFrame], 0, fadeOutDuration));
            StartCoroutine(SpriteFade(_framefade[_currentFrame], 1, fadeInDuration));

            StartCoroutine(MoveOverSeconds(transform, _frames[_currentFrame].position, moveDuration));
        }
    }

    IEnumerator MoveOverSeconds(Transform objectToMove, Vector3 end, float seconds)
    {
        DisableButtons();

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

        EnableButtons();
    }

    IEnumerator SpriteFade(SpriteRenderer sr, float endValue, float duration)
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
