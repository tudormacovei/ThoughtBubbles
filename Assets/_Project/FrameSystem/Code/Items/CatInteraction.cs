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

    private bool didInteract = false;

    IEnumerator DelaySpawnDialog(float seconds, int dialogIndex)
    {
        yield return new WaitForSeconds(seconds);
        DialogManager.Instance.SpawnDialog(dialogIndex);
        _anim.SetBool("IsPetting", false);
        
        StopAllCoroutines();
    }

    float Cooldown = 5.0f;
    float ElapsedTime = 0.0f;

    private void Update()
    {
        ElapsedTime += Time.deltaTime;
        if (ElapsedTime / Cooldown < 0.2f)
        {
            _popText.text = "Wait...";
        }
        else if (ElapsedTime / Cooldown < 0.5f)
        {
            _popText.text = "Wait..";
        }
        else if (ElapsedTime / Cooldown < 0.8f)
        {
            _popText.text = "Wait.";
        }
        else
        {
            _popText.text = "POP!";
        }
    }

    // TODO serialze?
    float AnimDuration = 3.0f;
    float elapsedAnimTime;
    Vector3 startPos;
    bool didRemoveBubble;

    IEnumerator AnimateBubblePop(Vector3 bubblePos, int BubbleIndex)
    {
        while (enabled)
        {
            if (elapsedAnimTime > AnimDuration)
            {
                // ON EXIT
                elapsedAnimTime = 0.0f;
                ElapsedTime = 0.0f;
                didRemoveBubble = false;
                _anim.SetBool("IsJumping", false);

                Debug.Log("Exiting Pop Function");
                StopAllCoroutines();
                yield break;
            }

            Debug.Log("Incrementing Time: " + elapsedAnimTime);
            elapsedAnimTime += Time.deltaTime;

            if (elapsedAnimTime / AnimDuration <= 0.2f) // sit still for the first part of the anim
            {
                Debug.Log("Branch 1: Sitting");
            }
            else if (elapsedAnimTime / AnimDuration <= 0.6f)
            {
                transform.position = Vector3.Lerp(startPos, bubblePos, ((elapsedAnimTime / AnimDuration) - 0.3f) * 3.0f);
                Debug.Log("Branch 2: Moving up");
            }
            else
            {
                Debug.Log("Branch 3: Going Down");
                if (!didRemoveBubble)
                {
                    BubbleManager.Instance.RemoveBubble(BubbleIndex);
                    didRemoveBubble = true;
                }
                transform.position = Vector3.Lerp(bubblePos, startPos, ((elapsedAnimTime / AnimDuration) - 0.6f) * 3.0f);
            }
            yield return 0; // Wait until next frame
        }
        
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (didInteract)
            {
                return;
            }

            _anim.SetBool("IsPetting", true);
            didInteract = true;

            StartCoroutine(DelaySpawnDialog(_delay, _dialogIndex));
        }
        else if (Input.GetMouseButtonDown(1) && ElapsedTime > Cooldown && BubbleManager.Instance.GetBubbleCount() > 0)
        {
            int bubbleIndex = BubbleManager.Instance.GetBubbleCount() - 1;

            var bubblePosition = BubbleManager.Instance.GetBubblePosition(bubbleIndex);
            Debug.Log(bubblePosition);

            startPos = transform.position;
            elapsedAnimTime = 0.0f;
            didRemoveBubble = false;
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
