using System.Collections;
using UnityEngine;

public class ItemInteraction : MonoBehaviour
{
    [SerializeField] int _dialogIndex;

    [SerializeField] SpriteRenderer _interactable;
    [SerializeField] SpriteRenderer _unInteractable;

    Collider2D _col;

    void Awake()
    {
        _col = GetComponent<Collider2D>();
    }

    void OnMouseDown()
    {
        if (FrameController.Instance.Moving) return;

        _col.enabled = false;

        StartCoroutine(SpriteFade(_interactable, 0, FrameController.Instance.FadeInDuration));
        StartCoroutine(FrameController.SpriteFade(_unInteractable, 1, FrameController.Instance.FadeInDuration));

        CatEvent.Instance.CountTrigger();

        DialogManager.Instance.SpawnDialog(_dialogIndex);
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

        sr.gameObject.SetActive(false);
    }
}
