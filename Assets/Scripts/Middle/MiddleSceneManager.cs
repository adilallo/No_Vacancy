using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class MiddleSceneManager : MonoBehaviour
{
    [HeaderAttribute("UI")]
    [SerializeField] private VideoPlayer npcVideoPlayer;
    [SerializeField] private GameObject npcRawImage;
    [SerializeField] private VideoPlayer arrowVideoPlayer;
    [SerializeField] private GameObject arrowLeftRawImage;
    [SerializeField] private GameObject arrowRightRawImage;
    [SerializeField] private List<VideoClip> videoClips;

    [HeaderAttribute("Audio")]
    [SerializeField] private List<AudioClip> middleSceneAudioClips;

    private int currentVideoIndex = 0;

    void Start()
    {
        npcRawImage.SetActive(false);
        arrowLeftRawImage.SetActive(false);
        arrowRightRawImage.SetActive(false);

        npcVideoPlayer.prepareCompleted += OnVideosPrepared;
        arrowVideoPlayer.prepareCompleted += OnVideosPrepared;

        currentVideoIndex = 0;

        npcVideoPlayer.Prepare();
        arrowVideoPlayer.Prepare();

        if (videoClips.Count > 0)
        {
            PlayVideo(currentVideoIndex);
            LeaderboardManager.Instance.SetVideoClips(videoClips);
        }
        else
        {
            Debug.LogWarning("No video clips assigned in the videoClips list.");
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPlaylist(middleSceneAudioClips);
        }
    }

    private void OnDisable()
    {
        npcVideoPlayer.prepareCompleted -= OnVideosPrepared;
        arrowVideoPlayer.prepareCompleted -= OnVideosPrepared;
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

    public void OnVideoSelected()
    {
        LeaderboardManager.Instance.RecordVideoSelection(currentVideoIndex);  // Use the LeaderboardManager to track selection
    }

    private void PlayVideo(int index)
    {
        if (videoClips.Count > 0 && videoClips[index] != null)
        {
            npcVideoPlayer.clip = videoClips[index];
            npcVideoPlayer.Play();
        }
    }

    private void OnVideosPrepared(VideoPlayer vp)
    {
        npcRawImage.SetActive(true);
        arrowLeftRawImage.SetActive(true);
        arrowRightRawImage.SetActive(true);
    }
}
