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
    [SerializeField] private List<AudioClip> npcAudioClips;
    [SerializeField] private List<AudioClip> middleSceneAudioClips;
    [SerializeField] private AudioSource audioSource;

    private int currentVideoIndex = 0;

    void Start()
    {
        npcRawImage.SetActive(false);
        arrowLeftRawImage.SetActive(false);
        arrowRightRawImage.SetActive(false);

        npcVideoPlayer.prepareCompleted += OnVideosPrepared;
        arrowVideoPlayer.prepareCompleted += OnVideosPrepared;

        currentVideoIndex = 0;


        arrowVideoPlayer.Prepare();

        if (videoClips.Count > 0)
        {
            PlayVideoAndAudio(currentVideoIndex);
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
        PlayVideoAndAudio(currentVideoIndex);
    }

    public void PreviousVideo()
    {
        currentVideoIndex = (currentVideoIndex - 1 + videoClips.Count) % videoClips.Count;
        PlayVideoAndAudio(currentVideoIndex);
    }

    public void OnVideoSelected()
    {
        LeaderboardManager.Instance.RecordVideoSelection(currentVideoIndex);
    }

    private void PlayVideoAndAudio(int index)
    {
        if (videoClips.Count > 0 && videoClips[index] != null)
        {
            npcVideoPlayer.clip = videoClips[index];
            npcVideoPlayer.Play();
        }

        if (npcAudioClips.Count > index && npcAudioClips[index] != null)
        {
            audioSource.clip = npcAudioClips[index];
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No audio clip assigned for video at index " + index);
        }
    }

    private void OnVideosPrepared(VideoPlayer vp)
    {
        npcRawImage.SetActive(true);
        arrowLeftRawImage.SetActive(true);
        arrowRightRawImage.SetActive(true);
    }
}
