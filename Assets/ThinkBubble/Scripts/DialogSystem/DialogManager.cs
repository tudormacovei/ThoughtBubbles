using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;

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
    [SerializeField] private int choiceCount;

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

    void Update()
    {
        
    }

    [ContextMenu("SpawnDialogTest")]
    void SpawnDialogTest()
    {
        SpawnDialog(choiceCount);
    }

    public void SpawnDialog(int choiceNum)
    {
        if (choiceNum <= 0)
        {
            Debug.LogError("The number of choices is 0 or less!");
            return;
        }

        GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(playerTransform.position);

        SpawnQuestion();

        for (int i = 0; i < choiceNum; i++)
        {
            GameObject choice = GetChoice();

            choice.GetComponentInChildren<TMP_Text>().text = i.ToString();

            choice.SetActive(true);

            int distanceY = i - choiceNum / 2;
            if (choiceNum % 2 == 0 && i >= choiceNum / 2) // even number correction
            {
                distanceY++;
            }

            StartCoroutine(choice.GetComponent<ChoiceMove>().CoAnimateButton(distanceY * choiceAnimOffset));
        }
    }

    void SpawnQuestion()
    {
        if(question == null)
        {
            question = Instantiate(questionPrefab, Vector2.zero, Quaternion.identity);
            question.transform.SetParent(transform);
        }
        question.GetComponent<RectTransform>().anchoredPosition = questionPositionOffset;

        question.SetActive(true);
    }

    #region Pooling Choice Object 
    GameObject GetChoice()
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
