using UnityEngine;
using UnityEngine.InputSystem;

public class WobbleTestManager : MonoBehaviour
{
    public WobbleEffect wobbleEffect;
    private Keyboard keyboard;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        keyboard = Keyboard.current;
    }

    // Update is called once per frame
    void Update()
    {
        if(keyboard.aKey.wasPressedThisFrame)
        {
            WobbleOn();
        }
        else if(keyboard.bKey.wasPressedThisFrame)
        {
            WobbleOff();
        }
    }

    private void WobbleOn()
    {
        wobbleEffect.StartWobble();
    }

    private void WobbleOff()
    {
        wobbleEffect.StopWobble();
    }
}
