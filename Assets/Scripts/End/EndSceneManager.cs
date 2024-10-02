using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class EndSceneManager : MonoBehaviour
{
    [HeaderAttribute("UI")]
    [SerializeField] private TMP_Text leaderboardText;

    [HeaderAttribute("Audio")]
    [SerializeField] private List<AudioClip> endSceneAudioClips;

    void Start()
    {
        leaderboardText.text = "";

        DisplayLeaderboard();

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPlaylist(endSceneAudioClips);
            AudioManager.Instance.OnPlaylistFinished += LoadFirstScene;
        }
    }

    void OnDisable()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.OnPlaylistFinished -= LoadFirstScene;
        }
    }

    private void DisplayLeaderboard()
    {
        Dictionary<int, int> videoSelections = LeaderboardManager.Instance.GetAllVideoSelections();
        List<VideoClip> videoClips = LeaderboardManager.Instance.GetVideoClips();

        List<KeyValuePair<int, int>> videoSelectionList = new List<KeyValuePair<int, int>>();

        foreach (var entry in videoSelections)
        {
            int videoIndex = entry.Key;
            int selectionCount = entry.Value;
            videoSelectionList.Add(new KeyValuePair<int, int>(videoIndex, selectionCount));
        }

        videoSelectionList.Sort((x, y) => y.Value.CompareTo(x.Value));

        for (int i = 0; i < videoSelectionList.Count; i++)
        {
            int videoIndex = videoSelectionList[i].Key;
            string videoName = videoClips[videoIndex].name;
            int selectionCount = videoSelectionList[i].Value;

            leaderboardText.text += videoName + " " + selectionCount + "\n";
        }
    }

    private void LoadFirstScene()
    {
        SceneManager.LoadScene("Start");
    }
}
