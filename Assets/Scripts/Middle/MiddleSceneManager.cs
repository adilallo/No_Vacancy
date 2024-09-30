using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class MiddleSceneManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private RawImage renderTextureUI;
    [SerializeField] private List<VideoClip> videoClips;

    public static Dictionary<int, int> videoSelections = new Dictionary<int, int>();
    public static MiddleSceneManager Instance { get; private set; }

    private int currentVideoIndex = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        for (int i = 0; i < videoClips.Count; i++)
        {
            if (!videoSelections.ContainsKey(i))
            {
                videoSelections[i] = 0; 
            }
        }

        if (videoClips.Count > 0)
        {
            PlayVideo(currentVideoIndex);
        }
        else
        {
            Debug.LogWarning("No video clips assigned in the videoClips list.");
        }
    }

    public List<VideoClip> GetVideoClips()
    {
        return videoClips; 
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
        if (videoSelections.ContainsKey(currentVideoIndex))
        {
            videoSelections[currentVideoIndex]++;
        }
        else
        {
            videoSelections[currentVideoIndex] = 1;
        }

        Debug.Log("Video " + currentVideoIndex + " was selected. Total selections: " + videoSelections[currentVideoIndex]);
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
