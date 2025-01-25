using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class FrameController : MonoBehaviour
{
    [SerializeField] Button _left;
    [SerializeField] Button _right;

    [SerializeField]
    Transform[] _frames;

    [SerializeField]
    float duration;

    int _currentFrame;

    bool _isMoving;

    public void MoveNext()
    {
        if (_currentFrame == _frames.Length - 1) return;

        if (!_isMoving)
        {
            _currentFrame++;

            StartCoroutine(MoveOverSeconds(transform, _frames[_currentFrame].position, duration));
        }
    }

    public void MovePrev()
    {
        if (_currentFrame == 0) return;

        if (!_isMoving)
        {
            _currentFrame--;

            StartCoroutine(MoveOverSeconds(transform, _frames[_currentFrame].position, duration));
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
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        objectToMove.transform.position = end;
        _isMoving = false;

        EnableButtons();
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
