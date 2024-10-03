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

        private void SetResolution(int width, int height, bool fullscreen)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            Screen.SetResolution(width, height, fullscreen);
        }
    }
}