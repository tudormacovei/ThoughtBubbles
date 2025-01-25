using UnityEngine;
using UnityEngine.UI;

public class ChoiceSelect : MonoBehaviour
{
    public int Damage = 0;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Select);
    }

    void Select()
    {
        Debug.Log(Damage);
        DialogManager.Instance.InactivateDialog();
    }
}
