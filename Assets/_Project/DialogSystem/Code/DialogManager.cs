using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;

    [Header("ScriptableObject")]
    [SerializeField] DialogScriptableObjectScript _dialogScriptableObject;

    [Header("Choice")]
    [SerializeField] GameObject _choicePrefab;
    [SerializeField] float _choiceAnimOffset;
    [SerializeField] List<GameObject> _activeChoiceList = new List<GameObject>();
    private Queue<GameObject> _choicePool = new Queue<GameObject>();

    [Header("Question")]
    [SerializeField] GameObject _questionPrefab;
    [SerializeField] GameObject _question;
    [SerializeField] Vector2 _questionPositionOffset;

    [Header("Player")]
    [SerializeField] Transform _playerTransform;

    [Header("Test")]
    [SerializeField] int _myNum; // TODO: is this necessary?

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

    [ContextMenu("SpawnDialogTest")]
    void SpawnDialogTest()
    {
        SpawnDialog(_myNum);
    }

    public void SpawnDialog(int dialogNum)
    {
        if (dialogNum < 0 || dialogNum >= _dialogScriptableObject.DialogDatas.Length)
        {
            Debug.LogError("The value of dialogNum is out of dialogScriptableObject.dialogDatas range!");
            return;
        }

        // not necessary, the dialogManager is responsible for its own position now
        // GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(playerTransform.position);

        SpawnQuestion(dialogNum);

        int choicesLength = _dialogScriptableObject.DialogDatas[dialogNum].choices.Length;

        for (int i = 0; i < choicesLength; i++)
        {
            GameObject choice = GetChoice(_dialogScriptableObject.DialogDatas[dialogNum].damageNumber[i]);

            choice.GetComponentInChildren<TMP_Text>().text = _dialogScriptableObject.DialogDatas[dialogNum].choices[i];

            choice.SetActive(true);

            int distanceY = i - choicesLength / 2;
            if (choicesLength % 2 == 0 && i >= choicesLength / 2) // even number correction
            {
                distanceY++;
            }

            StartCoroutine(choice.GetComponent<ChoiceMove>().CoAnimateButton(distanceY * _choiceAnimOffset));
        }
    }

    void SpawnQuestion(int dialogNum)
    {
        if(_question == null)
        {
            _question = Instantiate(_questionPrefab, Vector2.zero, Quaternion.identity);
            _question.transform.SetParent(transform);
        }
        _question.GetComponent<RectTransform>().anchoredPosition = _questionPositionOffset;
        _question.GetComponentInChildren<TMP_Text>().text = _dialogScriptableObject.DialogDatas[dialogNum].question;

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

        if (_choicePool.Count > 0)
        {
            choice = _choicePool.Dequeue();
        }
        else
        {
            choice = Instantiate(_choicePrefab, Vector2.zero, Quaternion.identity);
            choice.transform.SetParent(transform);
            
            if (choice.TryGetComponent(out ChoiceSelect cs)) {
                cs.Damage = damage;
            }
        }

        _activeChoiceList.Add(choice);
        return choice;
    }

    void ReleaseChoice(GameObject choice)
    {
        choice.SetActive(false);
        _choicePool.Enqueue(choice);
    }
    #endregion
}
