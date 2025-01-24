using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FrameController : MonoBehaviour
{
    [SerializeField]
    Button[] _input;

    [SerializeField]
    Transform[] _frames;

    [SerializeField]
    float speed;

    int _currentFrame;

    bool _isMoving;

    public void MoveFrame(int incIndex)
    {
        if (_currentFrame < 0 || _currentFrame >= _frames.Length - 1) return;

        if (!_isMoving)
        {
            _currentFrame += incIndex;

            StartCoroutine(MoveOverSeconds(transform, _frames[_currentFrame].position, speed));
        }
    }

    IEnumerator MoveOverSeconds(Transform objectToMove, Vector3 end, float seconds)
    {
        ActivateButtons(false);

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

        ActivateButtons(true);
    }

    void ActivateButtons(bool value)
    {
        for(int i = 0; i < _input.Length; i++)
        {
            _input[i].gameObject.SetActive(value);
        }
    }
}
