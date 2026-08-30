using UnityEngine;
using UnityEngine.EventSystems;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Camera Position")]
    public float distance = 5f;
    public float height = 2f;

    [Header("Camera Rotation")]
    public float sensitivity = 2f;
    public float minPitch = -30f;
    public float maxPitch = 60f;

    [Header("Smoothing")]
    public float positionSmoothness = 10f;

    [Header("Camera Shake")]
    public float shakeDuration = 0.15f;
    public float shakeAmount = 0.15f;

    private float yaw = 0f;
    private float pitch = 15f;

    private float shakeTimer;

    void Update()
    {
        HandleMouseLook();
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 targetPosition =
            target.position + Vector3.up * height;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition - GetCameraDirection() * distance,
            positionSmoothness * Time.deltaTime
        );

        transform.LookAt(targetPosition);

        if (shakeTimer > 0f)
        {
            transform.position +=
                Random.insideUnitSphere * shakeAmount;

            shakeTimer -= Time.deltaTime;
        }
    }

    void HandleMouseLook()
    {
        // DO NOT move the camera while the joystick is being dragged.
        if (MobileJoystick.IsDragging)
            return;

        // DO NOT move the camera while clicking/dragging other UI.
        if (EventSystem.current != null &&
            EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        RotateCamera(
            new Vector2(mouseX, mouseY)
        );
    }

    Vector3 GetCameraDirection()
    {
        Quaternion rotation =
            Quaternion.Euler(pitch, yaw, 0f);

        return rotation * Vector3.forward;
    }

    public void RotateCamera(Vector2 delta)
    {
        yaw += delta.x * sensitivity;
        pitch -= delta.y * sensitivity;

        pitch = Mathf.Clamp(
            pitch,
            minPitch,
            maxPitch
        );
    }

    public void ShakeCamera()
    {
        shakeTimer = shakeDuration;
    }
}