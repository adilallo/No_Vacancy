using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class MiddleSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject playButton;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private RawImage renderTextureUI;
    [SerializeField] private List<VideoClip> videoClips;

    private int currentVideoIndex = 0; 

    void Start()
    {
        if (videoClips.Count > 0)
        {
            PlayVideo(currentVideoIndex);
        }
        else
        {
            Debug.LogWarning("No video clips assigned in the videoClips list.");
        }
    }

    void Update()
    {
        playButton.transform.Rotate(0, (float)(24 * Time.deltaTime), 0);
    }

    public void NextVideo()
    {
        currentVideoIndex = (currentVideoIndex + 1) % videoClips.Count;
        PlayVideo(currentVideoIndex);
    }

    public void PreviousVideo()
    {
        currentVideoIndex = (currentVideoIndex - 1 + videoClips.Count) % videoClips.Count;
        PlayVideo(currentVideoIndex);
    }

    private void PlayVideo(int index)
    {
        if (videoClips.Count > 0 && videoClips[index] != null)
        {
            videoPlayer.clip = videoClips[index];
            videoPlayer.Play();
        }
    }
}
