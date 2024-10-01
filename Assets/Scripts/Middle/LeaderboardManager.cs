using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance { get; private set; }  // Singleton instance

    private Dictionary<int, int> videoSelections = new Dictionary<int, int>();  // Track selections by video index
    private List<VideoClip> videoClips = new List<VideoClip>();  // Store the video clips

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Ensure the leaderboard persists across scenes
        }
        else
        {
            Destroy(gameObject);  // Avoid duplicate instances
        }
    }

    // Method to set the video clips
    public void SetVideoClips(List<VideoClip> clips)
    {
        videoClips = clips;
    }

    // Method to get the video clips
    public List<VideoClip> GetVideoClips()
    {
        return videoClips;
    }

    // Method to increment the selection count for a specific video
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

    // Get the total number of selections for a specific video
    public int GetVideoSelectionCount(int videoIndex)
    {
        if (videoSelections.ContainsKey(videoIndex))
        {
            return videoSelections[videoIndex];
        }
        return 0;
    }

    // Get all video selections
    public Dictionary<int, int> GetAllVideoSelections()
    {
        return videoSelections;
    }
}
