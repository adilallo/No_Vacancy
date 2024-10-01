using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Video;

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
        }
    }

    private void DisplayLeaderboard()
    {
        List<VideoClip> videoClips = MiddleSceneManager.Instance.GetVideoClips();

        List<KeyValuePair<int, int>> videoSelectionList = new List<KeyValuePair<int, int>>();

        for (int i = 0; i < videoClips.Count; i++)
        {
            int selectionCount = MiddleSceneManager.videoSelections.ContainsKey(i) ? MiddleSceneManager.videoSelections[i] : 0;
            videoSelectionList.Add(new KeyValuePair<int, int>(i, selectionCount));
        }

        videoSelectionList.Sort((x, y) => y.Value.CompareTo(x.Value));

        for (int i = 0; i < videoSelectionList.Count; i++)
        {
            int videoIndex = videoSelectionList[i].Key;
            string videoName = videoClips[videoIndex].name;
            int selectionCount = videoSelectionList[i].Value;

            leaderboardText.text += (i + 1) + ". " + videoName + " " + selectionCount + "\n";
        }
    }
}
