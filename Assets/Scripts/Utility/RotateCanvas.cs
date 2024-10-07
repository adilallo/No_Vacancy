using UnityEngine;

public class RotateUIContainer : MonoBehaviour
{
    [SerializeField] private RectTransform uiContainer;

    void Start()
    {
        if (uiContainer != null)
        {
            Debug.Log("Rotating UI container...");
            uiContainer.rotation = Quaternion.Euler(0, 0, 90);  // Rotates the UI container by 90 degrees on the Z axis
            Debug.Log($"UI container rotation: {uiContainer.rotation.eulerAngles}");
        }
        else
        {
            Debug.LogWarning("UI container is not assigned.");
        }
    }
}