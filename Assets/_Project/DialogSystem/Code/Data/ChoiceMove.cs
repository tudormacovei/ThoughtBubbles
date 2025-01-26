using UnityEngine;
using System.Collections;

public class ChoiceMove : MonoBehaviour
{
    [SerializeField] float animOffset = 0.1f;

    public IEnumerator CoAnimateButton(float distanceY)
    {
        RectTransform rect = GetComponent<RectTransform>();

        rect.anchoredPosition = Vector2.zero;
        Vector2 goalPos = Vector2.zero;
        goalPos.y = distanceY;

        while (distanceY - rect.anchoredPosition.y < -animOffset || distanceY - rect.anchoredPosition.y > animOffset)
        {
            rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, goalPos, Time.deltaTime);

            yield return null;
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
