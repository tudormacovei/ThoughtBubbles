using UnityEngine;
using UnityEngine.UI;

public class ChoiceSelect : MonoBehaviour
{
    //Cat dies after x choices made!
    static int ChoicesMade = 0;

    public int Damage = 0;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Select);
    }

    void Select()
    {
        ChoicesMade++;
        Debug.Log(Damage);


        DialogManager.Instance.InactivateDialog();
    }
}
