using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class EndSceneManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup uiCanvasGroup;
    [SerializeField] private float fadeDuration = 2f;

    [Header("UI")]
    [SerializeField] private TMP_Text leaderboardText;

    [Header("Audio")]
    [SerializeField] private List<AudioClip> endSceneAudioClips;

    [Header("Scrolling")]
    [SerializeField] private float scrollSpeed = 50f;  // Speed at which the text scrolls
    private RectTransform leaderboardRectTransform;

    void Start()
    {
        leaderboardText.text = "";

        DisplayLeaderboard();

        if (uiCanvasGroup != null)
        {
            uiCanvasGroup.alpha = 0;
            StartCoroutine(FadeInUI());
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPlaylist(endSceneAudioClips, true);
            AudioManager.Instance.OnPlaylistFinished += LoadFirstScene;
        }

        leaderboardRectTransform = leaderboardText.GetComponent<RectTransform>();

        StartCoroutine(ScrollLeaderboardText());
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
        if (LeaderboardManager.Instance == null)
        {
            Debug.LogWarning("LeaderboardManager instance is missing.");
            return;
        }

        Dictionary<int, int> videoSelections = LeaderboardManager.Instance.GetAllVideoSelections();
        List<VideoClip> videoClips = LeaderboardManager.Instance.GetVideoClips();

        if (videoSelections == null || videoClips == null)
        {
            Debug.LogWarning("Leaderboard data is missing. Cannot display the leaderboard.");
            return;
        }

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

            leaderboardText.text += videoName + "\n" + selectionCount + "\n";
        }
    }

    private void LoadFirstScene()
    {
        SceneManager.LoadScene("Start");
    }

    private IEnumerator FadeInUI()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            uiCanvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        uiCanvasGroup.alpha = 1;
    }

    private IEnumerator ScrollLeaderboardText()
    {
        float startY = leaderboardRectTransform.rect.height * -1;
        float endY = leaderboardRectTransform.rect.height; 

        leaderboardRectTransform.anchoredPosition = new Vector2(leaderboardRectTransform.anchoredPosition.x, startY);

        while (true)
        {
            while (leaderboardRectTransform.anchoredPosition.y < endY)
            {
                leaderboardRectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
                yield return null;
            }

            leaderboardRectTransform.anchoredPosition = new Vector2(leaderboardRectTransform.anchoredPosition.x, startY);
        }
    }
}
