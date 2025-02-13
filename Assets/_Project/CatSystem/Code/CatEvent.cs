using UnityEngine;
using tdk.Systems;
using System.Collections;

public class CatEvent : Singleton<CatEvent>
{
    public static bool IsCutScene = false;

    [SerializeField] 
    int _triggerThreshold;

    int _triggerCounter;

    [SerializeField]
    Animator _anim;

    [Header("Ghost")]
    [SerializeField]
    SpriteRenderer _cat;

    [SerializeField]
    PlayCutscene _cutscene;

    [SerializeField]
    float _time;

    [SerializeField]
    float _height;

    public void CountTrigger()
    {
        _triggerCounter++;

        if (_triggerCounter >= _triggerThreshold)
        {
            IsCutScene = true;
            StartCoroutine(CatDiesEvent());
        }
    }

    IEnumerator CatDiesEvent()
    {
        _anim.SetBool("IsDead", true);

        yield return new WaitForSeconds(2);

        StartCoroutine(FrameController.SpriteFade(_cat, 0, 4));

        yield return new WaitForSeconds(2);

        _anim.SetBool("IsGhost", true);

        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        StartCoroutine(FrameController.SpriteFade(_cat, 1f, 2));

        StartCoroutine(MoveOverSeconds(_cat.gameObject.transform, _cat.gameObject.transform.position + new Vector3(0, _height, 0), _time));
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

        _cat.gameObject.SetActive(false);
        IsCutScene = false;
    }
}
