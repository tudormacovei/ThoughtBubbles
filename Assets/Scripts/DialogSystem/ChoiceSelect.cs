using UnityEngine;
using UnityEngine.UI;

public class ChoiceSelect : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Select);
    }

    void Select()
    {
        DialogManager.Instance.InactivateDialog();

        
    }
}
