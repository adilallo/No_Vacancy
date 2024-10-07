using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance { get; private set; }

    private Dictionary<int, int> videoSelections = new Dictionary<int, int>();
    private List<VideoClip> videoClips = new List<VideoClip>(); 

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

    public void SetVideoClips(List<VideoClip> clips)
    {
        videoClips = clips;
    }

    public List<VideoClip> GetVideoClips()
    {
        return videoClips;
    }

    public void RecordVideoSelection(int videoIndex)
    {
        if (videoSelections.ContainsKey(videoIndex))
        {
            videoSelections[videoIndex]++;
        }
        else
        {
            videoSelections[videoIndex] = 1;
        }

        Debug.Log("Video " + videoIndex + " selected. Total selections: " + videoSelections[videoIndex]);
    }

    public int GetVideoSelectionCount(int videoIndex)
    {
        if (videoSelections.ContainsKey(videoIndex))
        {
            return videoSelections[videoIndex];
        }
        return 0;
    }

    public Dictionary<int, int> GetAllVideoSelections()
    {
        return videoSelections;
    }
}
