using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Camera Position")]
    public float distance = 5f;
    public float height = 2f;

    [Header("Camera Rotation")]
    public float sensitivity = 0.05f;
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

    // True while the cinematic Timeline is controlling the camera.
    private bool cinematicMode = false;

    void Update()
    {
        // Don't allow normal camera controls during the cinematic.
        if (cinematicMode)
            return;

        HandleMouseLook();
    }

    void LateUpdate()
    {
        // Don't move the gameplay camera while Cinemachine
        // is controlling it for the cinematic.
        if (cinematicMode)
            return;

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
        // Right-click + mouse movement controls the camera.
        if (!Input.GetMouseButton(1))
            return;

        // Don't rotate the camera while using the joystick.
        if (MobileJoystick.IsDragging)
            return;

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
        // Ignore camera input during cinematic.
        if (cinematicMode)
            return;

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

    // Called when the cinematic starts.
    public void StartCinematic()
    {
        cinematicMode = true;
    }

    // Called when the cinematic finishes.
    public void EndCinematic()
    {
        cinematicMode = false;
    }
}