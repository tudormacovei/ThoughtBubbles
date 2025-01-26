using UnityEngine;
using UnityEngine.Video;

public class PlayCutscene : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Assign your VideoPlayer in the Inspector
    public GameObject videoSquare; // The square displaying the video
    public KeyCode triggerKey = KeyCode.P; // Key to trigger the video playback

    void Start()
    {
        // Make the square invisible initially
        videoSquare.SetActive(false);
        videoPlayer.Stop();

        // Add an event listener to hide the square when the video ends
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void Update()
    {
        if (Input.GetKeyDown(triggerKey))
        {
            PlayVideo();
        }
    }

    void PlayVideo()
    {
        videoSquare.SetActive(true);
        videoPlayer.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        StopVideo();
    }

    void StopVideo()
    {
        videoPlayer.Stop();
        videoSquare.SetActive(false);
    }
}

