using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace StartScene
{
    public class StartSceneManager : MonoBehaviour
    {
        [HeaderAttribute("Intro Assets")]
        [SerializeField] private RawImage introVideo;
        [SerializeField] private GameObject introImage;
        [SerializeField] private VideoPlayer introVideoPlayer;

        [HeaderAttribute("UI")]
        [SerializeField] private GameObject UI;
        [SerializeField] private VideoPlayer stockVideoPlayer;
        [SerializeField] private VideoPlayer avatarVideoPlayer;

        [HeaderAttribute("Audio")]
        [SerializeField] private List<AudioClip> startSceneAudioClips;

        private bool mouseClicked = false;

        void Start()
        {
            introImage.SetActive(true);
            introVideoPlayer.Prepare();
            stockVideoPlayer.Prepare();
            avatarVideoPlayer.Prepare();

            UI.SetActive(false);
            
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

        private void OnVideoFinished(VideoPlayer vp)
        {
            introVideoPlayer.time = 0;

            avatarVideoPlayer.Play();
            stockVideoPlayer.Play();  
            UI.SetActive(true);
        }
    }
}
