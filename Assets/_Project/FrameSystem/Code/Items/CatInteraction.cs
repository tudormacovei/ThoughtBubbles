using System.Collections;
using UnityEngine;

public class CatInteraction : MonoBehaviour
{
    [SerializeField] int _dialogIndex;

    [SerializeField] float _delay;

    [SerializeField] Animator _anim;

    [SerializeField] Collider2D _col;

    void OnMouseDown()
    {
        _anim.SetBool("IsPetting", true);
        _col.enabled = false;

        StartCoroutine(DelaySpawnDialog(_delay, _dialogIndex));
    }

    IEnumerator DelaySpawnDialog(float seconds, int dialogIndex)
    {
        yield return new WaitForSeconds(seconds);
        DialogManager.Instance.SpawnDialog(dialogIndex);
        _anim.SetBool("IsPetting", false);
    }
}
