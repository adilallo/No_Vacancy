using UnityEngine;
using UnityEngine.UI;

public class CustomCursor : MonoBehaviour
{
    [SerializeField] private Image cursorImage;
    private bool mouseMoved = false;

    void Start()
    {
        Cursor.visible = false;
        cursorImage.enabled = false;
    }

    void Update()
    {
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            if (!mouseMoved)
            {
                mouseMoved = true;
                cursorImage.enabled = true;
            }
        }

        if (cursorImage != null)
        {
            Vector2 cursorPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)cursorImage.transform.parent,
                Input.mousePosition,
                null,
                out cursorPosition
            );
            cursorImage.rectTransform.localPosition = cursorPosition;
        }
    }
}
