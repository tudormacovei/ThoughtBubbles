using Mono.Cecil;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CatInteraction : MonoBehaviour
{
    [SerializeField] int _dialogIndex;
    [SerializeField] float _delay;
    [SerializeField] Animator _anim;
    [SerializeField] Collider2D _col; // TODO: remove, this is deprecated
    [SerializeField] TextMeshProUGUI _popText;

    bool _didInteract = false;

    float _cooldown = 5.0f;
    float _elapsedTime = 0.0f;

    float _animDuration = 3.0f; // TODO: Serialize?
    float _elapsedAnimTime;
    Vector3 _startPos;
    bool _didRemoveBubble;

    IEnumerator DelaySpawnDialog(float seconds, int dialogIndex)
    {
        yield return new WaitForSeconds(seconds);
        DialogManager.Instance.SpawnDialog(dialogIndex);
        _anim.SetBool("IsPetting", false);
        
        StopAllCoroutines();
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime / _cooldown < 0.2f)
        {
            _popText.text = "Wait...";
        }
        else if (_elapsedTime / _cooldown < 0.5f)
        {
            _popText.text = "Wait..";
        }
        else if (_elapsedTime / _cooldown < 0.8f)
        {
            _popText.text = "Wait.";
        }
        else
        {
            _popText.text = "POP!";
        }
    }
    IEnumerator AnimateBubblePop(Vector3 bubblePos, int BubbleIndex)
    {
        while (enabled)
        {
            if (_elapsedAnimTime > _animDuration)
            {
                // ON EXIT
                _elapsedAnimTime = 0.0f;
                _elapsedTime = 0.0f;
                _didRemoveBubble = false;
                _anim.SetBool("IsJumping", false);

                Debug.Log("Exiting Pop Function");
                StopAllCoroutines();
                yield break;
            }

            Debug.Log("Incrementing Time: " + _elapsedAnimTime);
            _elapsedAnimTime += Time.deltaTime;

            if (_elapsedAnimTime / _animDuration <= 0.2f) // sit still for the first part of the anim
            {
                Debug.Log("Branch 1: Sitting");
            }
            else if (_elapsedAnimTime / _animDuration <= 0.6f)
            {
                transform.position = Vector3.Lerp(_startPos, bubblePos, ((_elapsedAnimTime / _animDuration) - 0.3f) * 3.0f);
                Debug.Log("Branch 2: Moving up");
            }
            else
            {
                Debug.Log("Branch 3: Going Down");
                if (!_didRemoveBubble)
                {
                    BubbleManager.Instance.RemoveBubble(BubbleIndex);
                    _didRemoveBubble = true;
                }
                transform.position = Vector3.Lerp(bubblePos, _startPos, ((_elapsedAnimTime / _animDuration) - 0.6f) * 3.0f);
            }
            yield return 0; // Wait until next frame
        }
        
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_didInteract)
            {
                return;
            }

            _anim.SetBool("IsPetting", true);
            _didInteract = true;

            StartCoroutine(DelaySpawnDialog(_delay, _dialogIndex));
        }
        else if (Input.GetMouseButtonDown(1) && _elapsedTime > _cooldown && BubbleManager.Instance.GetBubbleCount() > 0 && !IsCatAnimPlaying())
        {
            int bubbleIndex = BubbleManager.Instance.GetBubbleCount() - 1;

            var bubblePosition = BubbleManager.Instance.GetBubblePosition(bubbleIndex);
            Debug.Log(bubblePosition);

            _startPos = transform.position;
            _elapsedAnimTime = 0.0f;
            _didRemoveBubble = false;
            _anim.SetBool("IsJumping", true);
            StartCoroutine(AnimateBubblePop(bubblePosition, bubbleIndex));
        }
    }

    bool IsCatAnimPlaying()
    {
        if (_anim.GetBool("IsWalking") || _anim.GetBool("IsPetting") || _anim.GetBool("IsDead")
            || _anim.GetBool("IsGhost") || _anim.GetBool("IsJumping"))
        {
            return true;
        }
        return false;
    }
}
