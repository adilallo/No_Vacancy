using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace StartScene
{
    public class StartSceneManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup uiCanvasGroup;
        [SerializeField] private float fadeDuration = 2f;

        [HeaderAttribute("Intro Assets")]
        [SerializeField] private RawImage introVideo;
        [SerializeField] private GameObject introImage;
        [SerializeField] private VideoPlayer introVideoPlayer;

        [HeaderAttribute("UI")]
        [SerializeField] private GameObject UI;
        [SerializeField] private VideoPlayer stockVideoPlayer;
        [SerializeField] private RawImage stockRawImage;
        [SerializeField] private Image frameImage;
        [SerializeField] private VideoPlayer avatarVideoPlayer;
        [SerializeField] private GameObject selectButton;

        [HeaderAttribute("Audio")]
        [SerializeField] private List<AudioClip> startSceneAudioClips;

        private bool mouseClicked = false;
        private bool selectButtonVisible = false;
        private bool stockVideoStarted = false;

        void Start()
        {
            mouseClicked = false;
            selectButtonVisible = false;
            stockVideoStarted = false;

            introImage.SetActive(true);
            introVideoPlayer.Prepare();
            stockVideoPlayer.Prepare();
            avatarVideoPlayer.Prepare();

            if (uiCanvasGroup != null)
            {
                uiCanvasGroup.alpha = 0;
            }

            UI.SetActive(false);
            selectButton.SetActive(false);
            stockRawImage.color = new Color(stockRawImage.color.r, stockRawImage.color.g, stockRawImage.color.b, 0f);
            frameImage.color = new Color(stockRawImage.color.r, stockRawImage.color.g, stockRawImage.color.b, 0f);

            Cursor.visible = false;

            introVideoPlayer.loopPointReached += OnVideoFinished;

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayPlaylist(startSceneAudioClips, false);
            }
        }

        private void OnDisable()
        {
            introVideoPlayer.loopPointReached -= OnVideoFinished;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!mouseClicked)
                {
                    mouseClicked = true;
                    StartCoroutine(PlayVideoWhenReady()); 
                }
            }

            if (avatarVideoPlayer.isPlaying && !selectButtonVisible)
            {
                CheckAvatarVideoEnd();
            }

            if (avatarVideoPlayer.isPlaying && !stockVideoStarted)
            {
                CheckStartStockVideo();
            }
        }

        private void CheckAvatarVideoEnd()
        {
            if (avatarVideoPlayer.time >= avatarVideoPlayer.length * 0.93f)
            {
                selectButton.SetActive(true);
                selectButtonVisible = true;
            }
        }

        private void CheckStartStockVideo()
        {
            if (avatarVideoPlayer.time >= avatarVideoPlayer.length * 0.5625f)
            {
                StartCoroutine(FadeInStockVideo());
                stockVideoStarted = true;
            }
        }

        private IEnumerator PlayVideoWhenReady()
        {
            while (!introVideoPlayer.isPrepared)
            {
                yield return null;
            }

            introImage.SetActive(false);
            introVideoPlayer.Play();
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

        private IEnumerator FadeInStockVideo()
        {
            float elapsedTime = 0f;
            stockVideoPlayer.Play();

            while (elapsedTime < fadeDuration)
            {
                float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
                stockRawImage.color = new Color(stockRawImage.color.r, stockRawImage.color.g, stockRawImage.color.b, alpha);
                frameImage.color = new Color(stockRawImage.color.r, stockRawImage.color.g, stockRawImage.color.b, alpha);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            stockRawImage.color = new Color(stockRawImage.color.r, stockRawImage.color.g, stockRawImage.color.b, 1f);
            frameImage.color = new Color(stockRawImage.color.r, stockRawImage.color.g, stockRawImage.color.b, 1f);
        }

        private void OnVideoFinished(VideoPlayer vp)
        {
            introVideoPlayer.time = 0;
            StartCoroutine(FadeInUI());
            avatarVideoPlayer.Play();
            stockVideoPlayer.Play();  
            UI.SetActive(true);
        }
    }
}
