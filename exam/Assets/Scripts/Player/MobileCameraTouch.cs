using UnityEngine;

public class MobileCameraTouch : MonoBehaviour
{
    public ThirdPersonCamera cameraController;

    void Update()
    {
        // Mouse control for testing in Unity
        if (Input.GetMouseButton(0))
        {
            Vector2 mouseDelta = new Vector2(
                Input.GetAxis("Mouse X"),
                Input.GetAxis("Mouse Y")
            );

            cameraController.RotateCamera(mouseDelta * 10f);
        }

        // Touch control for mobile
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                cameraController.RotateCamera(touch.deltaPosition);
            }
        }
    }
}