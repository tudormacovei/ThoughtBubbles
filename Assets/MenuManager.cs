using System.Collections;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Vector3 MainMenuCameraLocation = Vector3.zero;
    [SerializeField] Vector3 SettingsCameraLocation = Vector3.zero;
    [SerializeField] Vector3 GameCameraLocation = Vector3.zero;
    [SerializeField] Vector3 TutorialCameraLocation = Vector3.zero;

    [SerializeField] float TransitionTime = 1.0f;

    float StartTime;
    Vector3 StartPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToMenu()
    {
        MoveTo(MainMenuCameraLocation);
    }

    public void MoveToSettings()
    {
        MoveTo(SettingsCameraLocation);
    }

    public void MoveToGame()
    {
        MoveTo(GameCameraLocation);
    }

    public void MoveToTutorial()
    {
        MoveTo(TutorialCameraLocation);
    }

    private void MoveTo(Vector3 position)
    {
        StopAllCoroutines(); // To stop any existing MoveToAsync execution
        StartTime = Time.time;
        StartPosition = Camera.main.transform.position;
        StartCoroutine(MovetoAsync(position));
    }

    private IEnumerator MovetoAsync(Vector3 position)
    {
        while (enabled && Time.time < StartTime + TransitionTime)
        {
            Camera.main.transform.position = Vector3.Lerp(StartPosition, position, Mathf.SmoothStep(0.0f, 1.0f, (Time.time - StartTime) / TransitionTime));
            yield return 1; // To go to next frame
        }
        yield break;
    }
}
