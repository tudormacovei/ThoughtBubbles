using UnityEngine;
using tdk.Systems;
using System.Collections;
using TMPro;

public class CatEvent : Singleton<CatEvent>
{
    public bool IsCutScene = false;
    int _triggerCounter;

    [Header("General")]
    [SerializeField] int _triggerThreshold;
    [SerializeField] Animator _anim;

    [Header("Ghost")]
    [SerializeField] SpriteRenderer _cat;
    [SerializeField] PlayCutscene _cutscene;
    [SerializeField] float _time;
    [SerializeField] float _height;
    [SerializeField] TextMeshProUGUI _popText;

    public void CountTrigger()
    {
        _triggerCounter++;

        if (_triggerCounter >= _triggerThreshold)
        {
            if (!GameManager.Instance.IsEnd)
            {
                IsCutScene = true;
                StartCoroutine(CatDiesEvent());
            }
        }
    }
        
    public void HandleGameEnd()
    {
        _popText.enabled = false;
    }

    IEnumerator CatDiesEvent()
    {
        _anim.SetBool("IsDead", true);

        yield return new WaitForSeconds(6.0f);

        StartCoroutine(FrameController.SpriteFade(_cat, 0, 2.0f));

        yield return new WaitForSeconds(2.0f);

        _anim.SetBool("IsGhost", true);

        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        StartCoroutine(FrameController.SpriteFade(_cat, 1.5f, 1.0f));

        StartCoroutine(MoveOverSeconds(_cat.gameObject.transform, _cat.gameObject.transform.position + new Vector3(0, _height, 0), _time));
        
        _popText.CrossFadeColor(Color.black, 3.0f, false, false); // Cat is dead, cannot pop bubbles anymore
    }

    IEnumerator MoveOverSeconds(Transform objectToMove, Vector3 end, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        while (elapsedTime < seconds)
        {
            objectToMove.transform.position = Vector3.Lerp(startingPos, end, elapsedTime / seconds);
            objectToMove.transform.position = new Vector3(objectToMove.transform.position.x, objectToMove.transform.position.y, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        objectToMove.transform.position = end;
        objectToMove.transform.position = new Vector3(objectToMove.transform.position.x, objectToMove.transform.position.y, 0);

        _cutscene.PlayVideo();

        yield return new WaitForSeconds(5);

        gameObject.SetActive(false);
        IsCutScene = false;

        // to speed up game end once Bubbles dies, and provide a sense of urgency
        // deal damage until player has 8 bubbles on screen
        int damage = 8 - BubbleManager.Instance.GetBubbleCount();
        if (damage > 0)
        {
            BubbleManager.Instance.HandleDamage(damage);
        }
    }
}
