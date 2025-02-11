using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;

    [Header("ScriptableObject")]
    [SerializeField] private DialogScriptableObjectScript dialogScriptableObject;

    [Header("Choice")]
    [SerializeField] private GameObject choicePrefab;
    [SerializeField] private float choiceAnimOffset;
    private Queue<GameObject> choicePool = new Queue<GameObject>();
    [SerializeField] private List<GameObject> activeChoiceList = new List<GameObject>(); 

    [Header("Question")]
    [SerializeField] private GameObject questionPrefab;
    [SerializeField] private GameObject question;
    [SerializeField] private Vector2 questionPositionOffset;

    [Header("Player")]
    public Transform playerTransform;

    [Header("Test")]
    [SerializeField] private int myNum;

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
        SpawnDialog(myNum);
    }

    public void SpawnDialog(int dialogNum)
    {
        if (dialogNum < 0 || dialogNum >= dialogScriptableObject.dialogDatas.Length)
        {
            Debug.LogError("The value of dialogNum is out of dialogScriptableObject.dialogDatas range!");
            return;
        }

        // not necessary, the dialogManager is responsible for its own position now
        // GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(playerTransform.position);

        SpawnQuestion(dialogNum);

        int choicesLength = dialogScriptableObject.dialogDatas[dialogNum].choices.Length;

        for (int i = 0; i < choicesLength; i++)
        {
            GameObject choice = GetChoice(dialogScriptableObject.dialogDatas[dialogNum].damageNumber[i]);

            choice.GetComponentInChildren<TMP_Text>().text = dialogScriptableObject.dialogDatas[dialogNum].choices[i];

            choice.SetActive(true);

            int distanceY = i - choicesLength / 2;
            if (choicesLength % 2 == 0 && i >= choicesLength / 2) // even number correction
            {
                distanceY++;
            }

            StartCoroutine(choice.GetComponent<ChoiceMove>().CoAnimateButton(distanceY * choiceAnimOffset));
        }
    }

    void SpawnQuestion(int dialogNum)
    {
        if(question == null)
        {
            question = Instantiate(questionPrefab, Vector2.zero, Quaternion.identity);
            question.transform.SetParent(transform);
        }
        question.GetComponent<RectTransform>().anchoredPosition = questionPositionOffset;
        question.GetComponentInChildren<TMP_Text>().text = dialogScriptableObject.dialogDatas[dialogNum].question;

        question.SetActive(true);
    }

    public void InactivateDialog()
    {
        question.SetActive(false);

        foreach(var choice in activeChoiceList)
        {
            ReleaseChoice(choice);
        }
        activeChoiceList.Clear();
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

        if (choicePool.Count > 0)
        {
            choice = choicePool.Dequeue();
        }
        else
        {
            choice = Instantiate(choicePrefab, Vector2.zero, Quaternion.identity);
            choice.transform.SetParent(transform);
            
            if (choice.TryGetComponent(out ChoiceSelect cs)) {
                cs.Damage = damage;
            }
        }

        activeChoiceList.Add(choice);
        return choice;
    }

    void ReleaseChoice(GameObject choice)
    {
        choice.SetActive(false);
        choicePool.Enqueue(choice);
    }
    #endregion
}
