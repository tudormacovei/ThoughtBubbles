using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CatInteraction : MonoBehaviour
{
    [SerializeField] int _dialogIndex;

    [SerializeField] Sprite _catSprite;
    [SerializeField] string _question;
    [SerializeField] List<string> _choices;
    [SerializeField] List<int> _damages;

    [SerializeField] float _delay;
    [SerializeField] Animator _anim;
    [SerializeField] TextMeshProUGUI _popText;

    [Header("Tutorial")]
    [SerializeField] TextMeshProUGUI _tutorialTextCat;
    [SerializeField] TextMeshProUGUI _tutorialTextPop;

    bool _didInteract = false;

    [SerializeField] float _popCooldown = 5.0f;
    float _elapsedTime = 0.0f;

    float _animDuration = 2.5f; // TODO: Serialize?
    float _elapsedAnimTime;
    Vector3 _startPos;
    bool _didRemoveBubble;

    IEnumerator HandleCatPettingAsync(float seconds)
    {
        _anim.SetBool("IsPetting", true);
        yield return new WaitForSeconds(seconds); // wait before spawning Dialog box
        
        if (!_didInteract)
        {
            DialogManager.Instance.SpawnDialog(_question, _choices, _damages, _catSprite);
            _didInteract = true;
        }

        yield return new WaitForSeconds(2.0f); // show the petting animation for 2 seconds
        _anim.SetBool("IsPetting", false);
        
        StopAllCoroutines();
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime / _popCooldown < 0.2f)
        {
            _popText.text = "WAIT......";
        }
        else if (_elapsedTime / _popCooldown < 0.5f)
        {
            _popText.text = "WAIT....";
        }
        else if (_elapsedTime / _popCooldown < 0.8f)
        {
            _popText.text = "WAIT..";
        }
        else
        {
            _popText.text = "POP!";
        }
    }

    IEnumerator AnimateBubblePop(Vector3 bubblePos, int BubbleIndex)
    {
        var adjustedAnimDuration = _animDuration - (BubbleIndex / 15.0f) * 1.5f;
        while (enabled)
        {
            if (_elapsedAnimTime > adjustedAnimDuration)
            {
                // ON EXIT
                _elapsedAnimTime = 0.0f;
                _didRemoveBubble = false;
                _anim.SetBool("IsJumping", false);
                _elapsedTime = 0.0f;
                Debug.Log("Exiting Pop Function");
                StopAllCoroutines();
                yield break;
            }

            Debug.Log("Incrementing Time: " + _elapsedAnimTime);
            _elapsedAnimTime += Time.deltaTime;

            if (_elapsedAnimTime / adjustedAnimDuration <= 0.2f) // sit still for the first part of the anim
            {
                _elapsedTime = 0.0f; // To update on-screen text immediately when cat is clicked on
                Debug.Log("Branch 1: Sitting");
            }
            else if (_elapsedAnimTime / adjustedAnimDuration <= 0.6f)
            {
                _anim.SetBool("IsJumping", true);
                _anim.SetBool("IsPreparing", false);
                transform.position = Vector3.Lerp(_startPos, bubblePos, ((_elapsedAnimTime / adjustedAnimDuration) - 0.3f) * 3.0f);
                Debug.Log("Branch 2: Moving up");
                if (_elapsedAnimTime / adjustedAnimDuration >= 0.5f)
                {
                    if (!_didRemoveBubble)
                    {
                        BubbleManager.Instance.PopClick(BubbleIndex);
                        _didRemoveBubble = true;
                    }
                }
            }
            else
            {
                Debug.Log("Branch 3: Going Down");

                transform.position = Vector3.Lerp(bubblePos, _startPos, ((_elapsedAnimTime / adjustedAnimDuration) - 0.6f) * 3.0f);
            }
            yield return 0; // Wait until next frame
        }
        
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0)) // Pet Bubbles
        {
            StartCoroutine(HandleCatPettingAsync(_delay));
        }
        else if (Input.GetMouseButtonDown(1) && _elapsedTime > _popCooldown && BubbleManager.Instance.GetBubbleCount() > 0 && !IsCatAnimPlaying())
        {
            int bubbleIndex = BubbleManager.Instance.GetBubbleCount() - 1;

            var bubblePosition = BubbleManager.Instance.GetBubblePosition(bubbleIndex);
            Debug.Log(bubblePosition);

            _startPos = transform.position;
            _elapsedAnimTime = 0.0f;
            _didRemoveBubble = false;
            _anim.SetBool("IsPreparing", true);
            StartCoroutine(AnimateBubblePop(bubblePosition, bubbleIndex));

            if (_tutorialTextCat.alpha > 0.9f)
            {
                _tutorialTextCat.CrossFadeAlpha(0.0f, 0.2f, false);
                _tutorialTextPop.CrossFadeAlpha(0.0f, 8.0f, false );
            }
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
