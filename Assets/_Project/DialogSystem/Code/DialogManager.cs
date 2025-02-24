using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;

    [Header("Choice")]
    [SerializeField] GameObject _choicePrefab;
    [SerializeField] float _choiceAnimOffset;
    [SerializeField] List<GameObject> _activeChoiceList = new List<GameObject>();

    [Header("Question")]
    [SerializeField] GameObject _questionPrefab;
    [SerializeField] GameObject _question;
    [SerializeField] Vector2 _questionPositionOffset;

    [Header("Player")]
    [SerializeField] Transform _playerTransform;

    bool _inDialog = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // returns: true if the Dialogue was spawned, false otherwise
    public bool SpawnDialog(string question, List<string> choices, List<int> damages, Sprite bigSprite)
    {
        if (_inDialog)
        {
            return false; // do not spawn a new dialog if we already in an unresolved dialog
        }
        _inDialog = true;
        SpawnQuestion(question);

        for (int i = 0; i < choices.Count; i++)
        {
            GameObject choice = GetChoice(damages[i]);

            choice.GetComponentInChildren<TMP_Text>().text = choices[i];
            choice.SetActive(true);

            int distanceY = i - choices.Count / 2;
            if (choices.Count % 2 == 0 && i >= choices.Count / 2) // even number correction
            {
                distanceY++;
            }
            StartCoroutine(choice.GetComponent<ChoiceMove>().CoAnimateButton(distanceY * _choiceAnimOffset));
        }
        return true;
    }

    void SpawnQuestion(string question)
    {
        if (_question == null)
        {
            _question = Instantiate(_questionPrefab, Vector2.zero, Quaternion.identity);
            _question.transform.SetParent(transform);
        }
        _question.GetComponent<RectTransform>().anchoredPosition = _questionPositionOffset;
        _question.GetComponentInChildren<TMP_Text>().text = question;

        _question.SetActive(true);
    }

    public void InactivateDialog()
    {
        _question.SetActive(false);

        foreach(var choice in _activeChoiceList)
        {
            ReleaseChoice(choice);
        }
        _activeChoiceList.Clear();

        if (CatEvent.Instance.isActiveAndEnabled)
        {
            CatEvent.Instance.CountTrigger(); // count towards cat trigger when choice is made
        }

        _inDialog = false;
    }

    public void Move(bool moveRight)
    {
        Vector3 offset = new Vector3(4.0f, 0.0f, 0.0f);
        if (moveRight)
        {
            transform.position += offset;
        }
        else
        {
            transform.position -= offset;
        }
    }

    #region Pooling Choice Object 
    GameObject GetChoice(int damage)
    {
        GameObject choice;

        choice = Instantiate(_choicePrefab, Vector2.zero, Quaternion.identity);
        choice.transform.SetParent(transform);
        
        choice.GetComponent<ChoiceSelect>().Damage = damage;

        _activeChoiceList.Add(choice);
        return choice;
    }

    void ReleaseChoice(GameObject choice)
    {
        choice.SetActive(false);
    }
    #endregion
}
