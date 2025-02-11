using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CatInteraction : MonoBehaviour
{
    [SerializeField] int _dialogIndex;

    [SerializeField] float _delay;

    [SerializeField] Animator _anim;

    [SerializeField] Collider2D _col;

    [SerializeField] TextMeshProUGUI _popText;

    IEnumerator DelaySpawnDialog(float seconds, int dialogIndex)
    {
        yield return new WaitForSeconds(seconds);
        DialogManager.Instance.SpawnDialog(dialogIndex);
        _anim.SetBool("IsPetting", false);
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

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _anim.SetBool("IsPetting", true);
            _col.enabled = false;

            StartCoroutine(DelaySpawnDialog(_delay, _dialogIndex));
        }
        else if (Input.GetMouseButtonDown(1) && ElapsedTime > Cooldown && BubbleManager.Instance.GetBubbleCount() > 0)
        {
            BubbleManager.Instance.RemoveBubble();
            ElapsedTime = 0.0f;
        }

    }
}
