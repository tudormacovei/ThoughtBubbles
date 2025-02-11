using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CatInteraction : MonoBehaviour
{
    [SerializeField] int _dialogIndex;

    [SerializeField] float _delay;

    [SerializeField] Animator _anim;

    [SerializeField] Collider2D _col;

    IEnumerator DelaySpawnDialog(float seconds, int dialogIndex)
    {
        yield return new WaitForSeconds(seconds);
        DialogManager.Instance.SpawnDialog(dialogIndex);
        _anim.SetBool("IsPetting", false);
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _anim.SetBool("IsPetting", true);
            _col.enabled = false;

            StartCoroutine(DelaySpawnDialog(_delay, _dialogIndex));
        }
        else if (Input.GetMouseButtonDown(1))
        {
            BubbleManager.Instance.RemoveBubble();
        }

    }
}
