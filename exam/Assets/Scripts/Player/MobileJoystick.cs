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
        joystickRadius =
            joystickBackground.rect.width / 2f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        IsDragging = true;
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBackground,
            eventData.position,
            eventData.pressEventCamera,
            out position
        );

        position = Vector2.ClampMagnitude(
            position,
            joystickRadius
        );

        joystickHandle.anchoredPosition = position;

        InputDirection =
            position / joystickRadius;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsDragging = false;

        joystickHandle.anchoredPosition = Vector2.zero;

        InputDirection = Vector2.zero;
    }
}