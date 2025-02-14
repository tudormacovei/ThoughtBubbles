using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class CatPopScript : MonoBehaviour
{
    public int BubbleIndex;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseOver()
    {
        Debug.Log(BubbleIndex);

        // Mark bubble for popping if user right-clicks
        if (Input.GetMouseButtonDown(1))
        {
            BubbleManager.Instance.PopClick(BubbleIndex);
        }
    }
}
