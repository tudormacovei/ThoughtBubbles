using UnityEngine;

public class ItemInteraction : MonoBehaviour
{
    GameObject _interactable;
    GameObject _unInteractable;

    void OnMouseDown()
    {
        _interactable.SetActive(false);
        _unInteractable.SetActive(true);
    }
}
