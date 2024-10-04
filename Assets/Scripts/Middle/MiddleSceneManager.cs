using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class MiddleSceneManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup uiCanvasGroup;
    [SerializeField] private float fadeDuration = 2f;

    [Header("UI")]
    [SerializeField] private VideoPlayer npcVideoPlayer;
    [SerializeField] private VideoPlayer avatarVideoPlayer;
    [SerializeField] private RectTransform npcRawImage;
    [SerializeField] private RectTransform avatarRawImage;
    [SerializeField] private VideoPlayer arrowVideoPlayer;
    [SerializeField] private GameObject arrowLeftRawImage;
    [SerializeField] private GameObject arrowRightRawImage;
    [SerializeField] private List<VideoClip> npcVideoClips;
    [SerializeField] private List<VideoClip> avatarVideoClips;
    [SerializeField] private Canvas canvas;

    [Header("Audio")]
    [SerializeField] private List<AudioClip> middleSceneAudioClips;

    private int currentVideoIndex = 0;
    private Vector2 avatarVelocity = new Vector2(100f, 100f);
    private RectTransform canvasRectTransform;  

    void Start()
    {
        Cursor.visible = false;

        npcRawImage.gameObject.SetActive(false);
        avatarRawImage.gameObject.SetActive(false);
        arrowLeftRawImage.SetActive(false);
        arrowRightRawImage.SetActive(false);

        if (uiCanvasGroup != null)
        {
            uiCanvasGroup.alpha = 0;
            StartCoroutine(FadeInUI());
        }

        currentVideoIndex = 0;

        npcVideoPlayer.Prepare();
        npcVideoPlayer.prepareCompleted += OnVideosPrepared;

        if (npcVideoClips.Count > 0)
        {
            PlayVideoAndAudio(currentVideoIndex);
            LeaderboardManager.Instance.SetVideoClips(npcVideoClips);
        }
        else
        {
            Debug.LogWarning("No video clips assigned in the npcVideoClips list.");
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPlaylist(middleSceneAudioClips, false);
        }

        // Assign the Canvas RectTransform
        if (canvas != null)
        {
            canvasRectTransform = canvas.GetComponent<RectTransform>();
        }
        else
        {
            Debug.LogError("Canvas is not assigned! Please assign a Canvas in the Inspector.");
        }

        // Check if avatarRawImage is assigned properly
        if (avatarRawImage == null)
        {
            Debug.LogError("avatarRawImage is not assigned! Please check the Inspector.");
        }
    }

    void Update()
    {
        MoveAvatarRawImage();  // Move the avatar every frame
    }

    private void OnDisable()
    {
        npcVideoPlayer.prepareCompleted -= OnVideosPrepared;
        arrowVideoPlayer.prepareCompleted -= OnVideosPrepared;
    }

    public void NextVideo()
    {
        currentVideoIndex = (currentVideoIndex + 1) % npcVideoClips.Count;
        PlayVideoAndAudio(currentVideoIndex);
    }

    public void PreviousVideo()
    {
        currentVideoIndex = (currentVideoIndex - 1 + npcVideoClips.Count) % npcVideoClips.Count;
        PlayVideoAndAudio(currentVideoIndex);
    }

    public void OnVideoSelected()
    {
        LeaderboardManager.Instance.RecordVideoSelection(currentVideoIndex);
    }

    private void PlayVideoAndAudio(int index)
    {
        if (npcVideoClips.Count > 0 && npcVideoClips[index] != null)
        {
            npcVideoPlayer.clip = npcVideoClips[index];
            npcVideoPlayer.Play();
        }

        if (avatarVideoClips.Count > index && avatarVideoClips[index] != null)
        {
            avatarVideoPlayer.clip = avatarVideoClips[index];
            avatarVideoPlayer.Play();
        }
        else
        {
            Debug.LogWarning("No audio clip assigned for video at index " + index);
        }
    }

    private void OnVideosPrepared(VideoPlayer vp)
    {
        avatarRawImage.gameObject.SetActive(true);
        npcRawImage.gameObject.SetActive(true);
        arrowLeftRawImage.SetActive(true);
        arrowRightRawImage.SetActive(true);
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

    private void MoveAvatarRawImage()
    {
        // Ensure both avatarRawImage and canvasRectTransform are not null
        if (avatarRawImage == null || canvasRectTransform == null)
        {
            Debug.LogError("Either avatarRawImage or canvasRectTransform is null. Movement cannot proceed.");
            return;
        }

        // Get the current position of the avatar
        Vector2 currentPosition = avatarRawImage.anchoredPosition;

        // Update the position based on the velocity
        currentPosition += avatarVelocity * Time.deltaTime;

        // Get the size of the canvas
        float canvasWidth = canvasRectTransform.rect.width;
        float canvasHeight = canvasRectTransform.rect.height;

        // Get the size of the avatar
        float avatarWidth = avatarRawImage.rect.width;
        float avatarHeight = avatarRawImage.rect.height;

        float yOffset = canvasHeight * 0.07f;  // Adjust this value as needed to push everything up

        float minX = -canvasWidth / 2 + avatarWidth / 2;
        float maxX = canvasWidth / 2 - avatarWidth / 2;
        float minY = (-canvasHeight / 2 + avatarHeight / 2) + yOffset;
        float maxY = (canvasHeight / 2 - avatarHeight / 2) + yOffset;

        // Check and reverse direction if hitting horizontal edges
        if (currentPosition.x < minX)
        {
            currentPosition.x = minX;
            avatarVelocity.x = Mathf.Abs(avatarVelocity.x);  // Move right
        }
        else if (currentPosition.x > maxX)
        {
            currentPosition.x = maxX;
            avatarVelocity.x = -Mathf.Abs(avatarVelocity.x);  // Move left
        }

        // Check and reverse direction if hitting vertical edges
        if (currentPosition.y < minY)
        {
            currentPosition.y = minY;
            avatarVelocity.y = Mathf.Abs(avatarVelocity.y);  // Move up
        }
        else if (currentPosition.y > maxY)
        {
            currentPosition.y = maxY;
            avatarVelocity.y = -Mathf.Abs(avatarVelocity.y);  // Move down
        }

        avatarRawImage.anchoredPosition = currentPosition;
    }
}
