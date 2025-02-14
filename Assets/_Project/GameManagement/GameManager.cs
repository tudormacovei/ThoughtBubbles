using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton pattern

    [SerializeField] Vector3 MainMenuCameraLocation = Vector3.zero;
    [SerializeField] Vector3 SettingsCameraLocation = Vector3.zero;
    [SerializeField] Vector3 GameCameraLocation = Vector3.zero;
    [SerializeField] Vector3 TutorialCameraLocation = Vector3.zero;
    [SerializeField] Vector3 EndScreenLocation = Vector3.zero;

    [SerializeField] PlayCutscene _endScene;
    [SerializeField] GameObject _endCoverObject;
    [SerializeField] float _endMoveDelay;

    [SerializeField] float TransitionTime = 1.0f;

    public bool IsEnd;
    

    float StartTime;
    Vector3 StartPosition;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IsEnd = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartGame()
    {
        BubbleManager.Instance.EnableAutoSpawnBubbles();
    }

    public bool CanModifyGameState()
    {
        return !(IsEnd || CatEvent.IsCutScene || BubbleManager.IsSpawning || FrameController.Instance.Moving); 
    }

    public void EndGame()
    {
        if (IsEnd)
        {
            return;
        }
        IsEnd = true;
        FrameController.Instance.DisableButtons();
        _endScene.transform.GetChild(0).position = FrameController.Instance.transform.position;
        _endScene.PlayVideo();

        MoveTo(EndScreenLocation, _endMoveDelay); // delay movement of camera to allow for end cutscene
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
        StartGame();
    }

    public void MoveToTutorial()
    {
        MoveTo(TutorialCameraLocation);
    }

    private void MoveTo(Vector3 position, float delay = 0.0f)
    {
        StopAllCoroutines(); // To stop any existing MoveToAsync execution
        StartTime = Time.time;
        StartPosition = Camera.main.transform.position;
        StartCoroutine(MovetoAsync(position, delay));
    }

    private IEnumerator MovetoAsync(Vector3 position, float delay)
    {
        while (enabled && Time.time < StartTime + TransitionTime)
        {
            if (delay >  0.01f)
            {
                yield return new WaitForSeconds(delay);
                
                delay = 0.0f;
                StartTime = Time.time;
            }
            float t = Mathf.SmoothStep(0.0f, 1.0f, (Time.time - StartTime) / TransitionTime);
            Camera.main.transform.position = Vector3.Lerp(StartPosition, position, t);
            yield return 1; // To go to next frame
        }
        if (IsEnd)
        {
            _endCoverObject.SetActive(true);
        }
        yield break;
    }
}
