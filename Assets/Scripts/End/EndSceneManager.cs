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
    [SerializeField] private RectTransform parentPanelRectTransform;

    [Header("Video")]
    [SerializeField] private VideoPlayer avatarVideoPlayer;  // Avatar Video Player
    [SerializeField] private CanvasGroup avatarCanvasGroup;  // CanvasGroup to control avatar video fade

    [Header("Audio")]
    [SerializeField] private List<AudioClip> endSceneAudioClips;

    [Header("Scrolling")]
    [SerializeField] private float scrollSpeed = 50f;
    private RectTransform leaderboardRectTransform;

    void Start()
    {
        leaderboardText.text = "";

        DisplayLeaderboard();

        avatarVideoPlayer.Prepare();

        leaderboardRectTransform = leaderboardText.GetComponent<RectTransform>();

        // Ensure the UI is invisible initially
        uiCanvasGroup.alpha = 0;

        // Fade out the previous scene's audio
        if (AudioManager.Instance != null)
        {
            StartCoroutine(AudioManager.Instance.FadeOutCurrentTrack());  // Fade out middle scene audio
        }

        // Start the avatar video fade-in and play process
        StartCoroutine(PlayAvatarVideo());
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

    private IEnumerator PlayAvatarVideo()
    {
        // Fade in the avatar video
        float elapsedTime = 0f;
        avatarCanvasGroup.alpha = 0;
        avatarVideoPlayer.Play();

        while (elapsedTime < fadeDuration)
        {
            avatarCanvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        avatarCanvasGroup.alpha = 1;

        // Wait until the avatar video is done playing
        yield return new WaitUntil(() => avatarVideoPlayer.isPlaying == false);

        // Fade out the avatar video
        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            avatarCanvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        avatarCanvasGroup.alpha = 0;

        // After the avatar video is done, fade in the UI and start the audio playlist
        StartCoroutine(FadeInUI());
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPlaylist(endSceneAudioClips, true);
            AudioManager.Instance.OnPlaylistFinished += LoadFirstScene;
        }

        StartCoroutine(ScrollLeaderboardText());
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
        yield return null; // Ensure layout updates before proceeding
        leaderboardRectTransform.ForceUpdateRectTransforms();

        float textHeight = leaderboardRectTransform.rect.height;
        float parentHeight = parentPanelRectTransform.rect.height;

        float startY = -textHeight;
        float endY = parentHeight + textHeight;

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