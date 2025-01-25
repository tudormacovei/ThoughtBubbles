using UnityEngine;

public class ItemSwitcher : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] _shadedItems;
    [SerializeField] SpriteRenderer[] _coloredItems;

    public void DisableFrameFast()
    {
        for (int i = 0; i < _shadedItems.Length; i++)
        {
            StartCoroutine(FrameController.SpriteFade(_shadedItems[i], 0, 0.1f));
        }

        for (int i = 0; i < _coloredItems.Length; i++)
        {
            StartCoroutine(FrameController.SpriteFade(_coloredItems[i], 0, 0.1f));
        }
    }

    public void DisableFrame()
    {
        for (int i = 0; i < _shadedItems.Length; i++)
        {
            StartCoroutine(FrameController.SpriteFade(_shadedItems[i], 0, FrameController.Instance.FadeInDuration));
        }

        for (int i = 0; i < _coloredItems.Length; i++)
        {
            StartCoroutine(FrameController.SpriteFade(_coloredItems[i], 0, FrameController.Instance.FadeInDuration));
        }
    }
    
    public void EnableFrame()
    {
        for (int i = 0; i < _coloredItems.Length; i++)
        {
            StartCoroutine(FrameController.SpriteFade(_coloredItems[i], 1, FrameController.Instance.FadeInDuration));
        }
    }
}
