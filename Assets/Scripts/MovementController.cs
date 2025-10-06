using UnityEngine;

public class HelixController : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 300f;

    [Header("Helix Container")]
    [SerializeField] private Transform HelixContainer; 

    private float lastMouseX;
    private bool isDragging = false;

    void Update()
    {
        //Mouse Input
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastMouseX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging && Input.mousePresent)
        {
            float XChange = Input.mousePosition.x - lastMouseX;
            lastMouseX = Input.mousePosition.x;
            HelixContainer.Rotate(0f, -XChange * rotationSpeed * Time.deltaTime, 0f, Space.World);
        }


        //Touch Input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                isDragging = true;
                lastMouseX = touch.position.x;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }

            if (isDragging)
            {
                float XChange = touch.deltaPosition.x;
                HelixContainer.Rotate(0f, -XChange * rotationSpeed * Time.deltaTime, 0f, Space.World);
            }
        }
    }
}
