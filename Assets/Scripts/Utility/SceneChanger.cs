using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utility
{
    public class SceneChanger : MonoBehaviour
    {
        [SerializeField] private string firstSceneName;
        [SerializeField] private string secondSceneName;
        [SerializeField] private string thirdSceneName;

        private readonly int targetWidth = 800;
        private readonly int targetHeight = 1280;

        void Start()
        {
            if (Screen.width != targetWidth || Screen.height != targetHeight || !Screen.fullScreen)
            {
                SetResolution(targetWidth, targetHeight, true);
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ExitGame();
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

        public void LoadThirdScene()
        {
            if (!string.IsNullOrEmpty(thirdSceneName))
            {
                SceneManager.LoadScene(thirdSceneName);
            }
            else
            {
                Debug.LogWarning("Third scene name is not assigned.");
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

        private void SetResolution(int targetWidth, int targetHeight, bool fullscreen)
        {
            // Get the current screen width and height
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            // Calculate the aspect ratios
            float targetAspect = (float)targetWidth / (float)targetHeight;
            float screenAspect = screenWidth / screenHeight;

            // Determine if we need to adjust width or height to preserve the aspect ratio
            if (screenAspect > targetAspect)
            {
                // Screen is wider than target, adjust width
                int adjustedWidth = Mathf.RoundToInt(targetHeight * screenAspect);
                Screen.SetResolution(adjustedWidth, targetHeight, fullscreen);
            }
            else
            {
                // Screen is taller than target, adjust height
                int adjustedHeight = Mathf.RoundToInt(targetWidth / screenAspect);
                Screen.SetResolution(targetWidth, adjustedHeight, fullscreen);
            }

            Screen.fullScreenMode = fullscreen ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed;
        }
    }
}