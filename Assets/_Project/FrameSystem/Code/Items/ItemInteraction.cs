using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteraction : MonoBehaviour
{
    [SerializeField] int _dialogIndex; // This will be made legacy

    [SerializeField] string _question;
    [SerializeField] List<string> _choices;
    [SerializeField] List<int> _damages;

    [SerializeField] SpriteRenderer _interactable;
    [SerializeField] SpriteRenderer _unInteractable;

    Collider2D _col;

    void Awake()
    {
        _col = GetComponent<Collider2D>();
    }

    void OnMouseDown()
    {
        if (!GameManager.Instance.CanModifyGameState())
        {
            return;
        }

        _col.enabled = false;

        StartCoroutine(SpriteFade(_interactable, 0, FrameController.Instance.FadeInDuration));
        StartCoroutine(FrameController.SpriteFade(_unInteractable, 1, FrameController.Instance.FadeInDuration));

        FrameController.Instance.DisableButtons();
        DialogManager.Instance.SpawnDialog(_question, _choices, _damages, _interactable.sprite);
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
