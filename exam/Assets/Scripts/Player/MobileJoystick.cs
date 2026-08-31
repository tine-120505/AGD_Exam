using UnityEngine;
using UnityEngine.EventSystems;

public class MobileJoystick : MonoBehaviour,
    IPointerDownHandler,
    IDragHandler,
    IPointerUpHandler
{
    [Header("Joystick")]
    public RectTransform joystickBackground;
    public RectTransform joystickHandle;

    public Vector2 InputDirection { get; private set; }

    public static bool IsDragging { get; private set; }

    private float joystickRadius;

    void Start()
    {
        // Use the actual displayed size of the joystick.
        joystickRadius =
            Mathf.Min(
                joystickBackground.rect.width,
                joystickBackground.rect.height
            ) / 2f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        IsDragging = true;

        UpdateJoystickPosition(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateJoystickPosition(eventData);
    }

    private void UpdateJoystickPosition(PointerEventData eventData)
    {
        Vector2 localPosition;

        // Convert the touch/mouse position into the
        // joystick background's local coordinate system.
        bool success =
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                joystickBackground,
                eventData.position,
                eventData.pressEventCamera,
                out localPosition
            );

        if (!success)
            return;

        // Calculate the joystick's actual center.
        Vector2 center =
            new Vector2(
                (0.5f - joystickBackground.pivot.x) *
                joystickBackground.rect.width,

                (0.5f - joystickBackground.pivot.y) *
                joystickBackground.rect.height
            );

        // Position relative to the visual center.
        Vector2 offset =
            localPosition - center;

        // Keep the handle inside the joystick.
        offset = Vector2.ClampMagnitude(
            offset,
            joystickRadius
        );

        // Convert back to the handle's anchored position.
        joystickHandle.anchoredPosition =
            center + offset;

        // Convert to normalized movement input.
        InputDirection =
            offset / joystickRadius;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsDragging = false;

        // Return the handle to the visual center.
        Vector2 center =
            new Vector2(
                (0.5f - joystickBackground.pivot.x) *
                joystickBackground.rect.width,

                (0.5f - joystickBackground.pivot.y) *
                joystickBackground.rect.height
            );

        joystickHandle.anchoredPosition = center;

        InputDirection = Vector2.zero;
    }
}