using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceSelect : MonoBehaviour
{
    [SerializeField] GameObject _choiceVisualization;
    [SerializeField] Color _goodChoiceColor;
    [SerializeField] Color _badChoiceColor;

    public int Damage = 0;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Select);
    }

    void Select()
    {
        Debug.Log(Damage);

        // parent to the choice's parent
        var obj = Instantiate(_choiceVisualization, transform.position, Quaternion.identity, transform.parent);
        obj.GetComponent<TextMeshProUGUI>().text = "" + Damage;
        obj.GetComponent<TextMeshProUGUI>().color = (Damage > 0) ? _badChoiceColor : _goodChoiceColor; 

        FrameController.Instance.EnableButtons();

        BubbleManager.Instance.HandleDamage(Damage);

        DialogManager.Instance.InactivateDialog();
    }
}
