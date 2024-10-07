using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Button3D : MonoBehaviour
{
    private Button button;

    void Start()
    {
        // Get the Button component attached to the GameObject
        button = GetComponent<Button>();

        // Make sure the object has a Collider for raycast detection
        if (GetComponent<Collider>() == null)
        {
            // Add a BoxCollider if no Collider exists
            gameObject.AddComponent<BoxCollider>();
        }
    }

    void Update()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Perform a 3D physics raycast to detect the button, even from the back
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == this.transform)
                {
                    // Manually trigger the Button's onClick event
                    if (button != null)
                    {
                        button.onClick.Invoke();
                        Debug.Log("Button clicked from either side (via Physics Raycast)!");
                    }
                }
            }
        }
    }
}
