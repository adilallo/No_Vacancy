using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

namespace StartScene
{
    public class StartSceneManager : MonoBehaviour
    {
        public VideoPlayer videoPlayer;  
        public VideoClip initialClip;   
        public VideoClip secondClip; 
        public RawImage renderTextureUI;
        public GameObject imageUI;

        private bool mouseMoved = false;
        private bool secondVideoPlayed = false;

        void Start()
        {
            videoPlayer.clip = initialClip;
            videoPlayer.Play();

            Cursor.visible = false;

            imageUI.SetActive(false);

            videoPlayer.loopPointReached += OnVideoFinished;
        }

        void Update()
        {
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                if (!mouseMoved)
                {
                    mouseMoved = true;
                    SwitchToSecondClip();
                }
            }
        }

        private void SwitchToSecondClip()
        {
            videoPlayer.Stop();
            videoPlayer.clip = secondClip;
            videoPlayer.Play();
        }

        private void OnVideoFinished(VideoPlayer vp)
        {
            if (vp.clip == secondClip && !secondVideoPlayed)
            {
                Cursor.visible = true;
                imageUI.SetActive(true);
                secondVideoPlayed = true;
            }
        }
    }
}
