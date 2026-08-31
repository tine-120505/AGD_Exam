using UnityEngine;

public class MobileCameraTouch : MonoBehaviour
{
    public ThirdPersonCamera cameraController;

    private int cameraFingerId = -1;

    void Update()
    {
        // Look through all active touches.
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            // Find a finger that started on the right side.
            if (touch.phase == TouchPhase.Began)
            {
                if (touch.position.x > Screen.width / 2f)
                {
                    cameraFingerId = touch.fingerId;
                }
            }

            // Only this finger controls the camera.
            if (touch.fingerId == cameraFingerId)
            {
                if (touch.phase == TouchPhase.Moved)
                {
                    cameraController.RotateCamera(
                        touch.deltaPosition
                    );
                }

                if (touch.phase == TouchPhase.Ended ||
                    touch.phase == TouchPhase.Canceled)
                {
                    cameraFingerId = -1;
                }
            }
        }
    }
}