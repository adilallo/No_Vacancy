using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

namespace StartScene
{
    public class StartSceneManager : MonoBehaviour
    {
        [HeaderAttribute("Intro Assets")]
        [SerializeField] private GameObject introImage;
        [SerializeField] private GameObject introVideo;
        [SerializeField] private VideoPlayer introVideoPlayer;

        [HeaderAttribute("UI")]
        [SerializeField] private GameObject UI;
        [SerializeField] private VideoPlayer stockVideoPlayer;
        [SerializeField] private VideoPlayer avatarVideoPlayer;
        [SerializeField] private GameObject playButton;
        
        private bool mouseMoved = false;

        void Start()
        {
            introImage.SetActive(true);
            introVideoPlayer.Prepare();
            introVideo.SetActive(false);

            stockVideoPlayer.Prepare();
            avatarVideoPlayer.Prepare();
            UI.SetActive(false);

            UnityEngine.Cursor.visible = false;

            introVideoPlayer.loopPointReached += OnVideoFinished;
        }

        private void OnDisable()
        {
            introVideoPlayer.loopPointReached -= OnVideoFinished;
        }
        
        void Update()
        {
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                if (!mouseMoved)
                {
                    mouseMoved = true;
                    StartCoroutine(PlayVideoWhenReady());
                }
            }

            if (UI.activeSelf)
            {
                playButton.transform.Rotate(0, (float)(24 * Time.deltaTime), 0);
            }
        }

        private IEnumerator PlayVideoWhenReady()
        {
            while (!introVideoPlayer.isPrepared)
            {
                yield return null;
            }

            introImage.SetActive(false);
            introVideo.SetActive(true);

            introVideoPlayer.Play();
        }

        private void OnVideoFinished(VideoPlayer vp)
        {
            UnityEngine.Cursor.visible = true;

            avatarVideoPlayer.Play();
            stockVideoPlayer.Play();  
            UI.SetActive(true);
        }
    }
}
