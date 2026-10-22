
using UnityEngine;
using UnityEngine.EventSystems;

public class TruckTouchButton : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler,
    IPointerExitHandler
{
    public enum ControlType
    {
        Throttle,
        Brake,
        SteerLeft,
        SteerRight
    }

    [Header("Control Setup")]
    [SerializeField] private TruckController truck;
    [SerializeField] private ControlType controlType;

    public void OnPointerDown(PointerEventData eventData)
    {
        ApplyControl(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ApplyControl(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ApplyControl(false);
    }

    private void OnDisable()
    {
        ApplyControl(false);
    }

    private void ApplyControl(bool pressed)
    {
        if (truck == null)
            return;

        switch (controlType)
        {
            case ControlType.Throttle:
                truck.SetThrottle(pressed ? 1f : 0f);
                break;

            case ControlType.Brake:
                truck.SetBrake(pressed ? 1f : 0f);
                break;

            case ControlType.SteerLeft:
                truck.SetSteering(pressed ? -1f : 0f);
                break;

            case ControlType.SteerRight:
                truck.SetSteering(pressed ? 1f : 0f);
                break;
        }
    }
}
