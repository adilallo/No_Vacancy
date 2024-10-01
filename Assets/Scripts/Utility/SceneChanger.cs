using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utility
{
    public class SceneChanger : MonoBehaviour
    {
        [SerializeField] private string firstSceneName;
        [SerializeField] private string secondSceneName;

        private bool isFullScreen = false;

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
    }
}
