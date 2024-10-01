using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace Utility
{
    public class SceneChanger : MonoBehaviour
    {
        [SerializeField] private SceneAsset firstSceneAsset;
        [SerializeField] private SceneAsset secondSceneAsset;

        public void LoadFirstScene()
        {
            if (firstSceneAsset != null)
            {
                SceneManager.LoadScene(firstSceneAsset.name);
            }
            else
            {
                Debug.LogWarning("First scene asset is not assigned.");
            }
        }

        public void LoadSecondScene()
        {
            if (secondSceneAsset != null)
            {
                SceneManager.LoadScene(secondSceneAsset.name);
            }
            else
            {
                Debug.LogWarning("Second scene asset is not assigned.");
            }
        }
    }
}
