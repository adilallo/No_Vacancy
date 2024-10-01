using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utility
{
    public class SceneChanger : MonoBehaviour
    {
        [SerializeField] private string firstSceneName;
        [SerializeField] private string secondSceneName;

        private readonly int targetWidth = 800;
        private readonly int targetHeight = 1280;
        private bool isFullScreen = false;

        void Start()
        {
            SetResolution(targetWidth, targetHeight, false);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ExitGame();
            }

            if (Input.GetKeyDown(KeyCode.F11))
            {
                ToggleFullScreen();
            }
        }


        public void LoadFirstScene()
        {
            if (!string.IsNullOrEmpty(firstSceneName))
            {
                SceneManager.LoadScene(firstSceneName); 
            }
            else
            {
                Debug.LogWarning("First scene name is not assigned.");
            }
        }

        public void LoadSecondScene()
        {
            if (!string.IsNullOrEmpty(secondSceneName))
            {
                SceneManager.LoadScene(secondSceneName); 
            }
            else
            {
                Debug.LogWarning("Second scene name is not assigned.");
            }
        }

        private void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void ToggleFullScreen()
        {
            isFullScreen = !isFullScreen;
            if (isFullScreen)
            {
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                Screen.fullScreen = true;
            }
            else
            {
                Screen.fullScreenMode = FullScreenMode.Windowed;
                Screen.fullScreen = false;
            }
        }

        private void SetResolution(int width, int height, bool fullscreen)
        {
            Screen.SetResolution(width, height, fullscreen);
        }
    }
}
