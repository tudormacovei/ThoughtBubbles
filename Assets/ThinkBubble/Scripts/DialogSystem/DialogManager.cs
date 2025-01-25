using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

public class DialogManager : MonoBehaviour
{
    [Header("Choice")]
    [SerializeField] private GameObject choicePrefab;
    [SerializeField] private float offset;
    private Queue<GameObject> choicePool = new Queue<GameObject>();

    [Header("Player")]
    public Transform playerTransform;

    void Start()
    {

    }

    void Update()
    {
        
    }

    [ContextMenu("CreateChoice")]
    void CreateChoiceTest()
    {
        CreateChoice(3);
    }

    void CreateChoice(int choiceNum)
    {
        GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(playerTransform.position);

        for(int i = 0; i < choiceNum; i++)
        {
            GameObject choice = GetChoice();

            choice.GetComponentInChildren<TMP_Text>().text = i.ToString();

            choice.SetActive(true);

            int distanceY = i - choiceNum / 2;
            if (choiceNum % 2 == 0 && i >= choiceNum / 2) // even number correction
            {
                distanceY++;
            }

            StartCoroutine(choice.GetComponent<ChoiceMove>().CoAnimateButton(distanceY * offset));
        }

    }
 
    GameObject GetChoice()
    {
        if (choicePool.Count > 0)
        {
            return choicePool.Dequeue();
        }
        else
        {
            GameObject choice = Instantiate(choicePrefab, Vector2.zero, Quaternion.identity);
            choice.transform.SetParent(transform);

            return choice;
        }
    }

    void ReleaseChoice(GameObject choice)
    {
        choice.SetActive(false);
        choicePool.Enqueue(choice);
    }
}
