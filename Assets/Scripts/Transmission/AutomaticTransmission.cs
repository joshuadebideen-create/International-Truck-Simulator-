
using UnityEngine;

public class AutomaticTransmission : MonoBehaviour
{
    public enum GearMode
    {
        Reverse = -1,
        Neutral = 0,
        Drive = 1
    }

    [Header("Configuration")]
    [SerializeField]
    private TransmissionDefinition transmission;

    [Header("Current State")]
    [SerializeField]
    private GearMode mode = GearMode.Drive;

    [SerializeField, Min(1)]
    private int currentGear = 1;

    [Header("Engine")]
    [SerializeField, Min(500f)]
    private float engineRPM = 700f;

    [SerializeField, Min(500f)]
    private float idleRPM = 600f;

    [SerializeField, Min(500f)]
    private float maximumRPM = 2200f;

    public GearMode Mode => mode;
    public int CurrentGear => currentGear;
    public float EngineRPM => engineRPM;

    public float CurrentGearRatio
    {
        get
        {
            if (transmission == null)
                return 0f;

            if (mode == GearMode.Reverse)
                return -transmission.GetGearRatio(1);

            if (mode == GearMode.Neutral)
                return 0f;

            return transmission.GetGearRatio(currentGear);
        }
    }

    public void SelectDrive()
    {
        mode = GearMode.Drive;
        currentGear = Mathf.Clamp(
            currentGear, 1, GetMaximumGear());
    }

    public void SelectNeutral()
    {
        mode = GearMode.Neutral;
    }

    public void SelectReverse()
    {
        mode = GearMode.Reverse;
    }

    public void SetEngineRPM(float rpm)
    {
        engineRPM = Mathf.Clamp(
            rpm, idleRPM, maximumRPM);
    }

    public void ShiftUp()
    {
        if (mode == GearMode.Drive)
            currentGear = Mathf.Min(
                currentGear + 1, GetMaximumGear());
    }

    public void ShiftDown()
    {
        if (mode == GearMode.Drive)
            currentGear = Mathf.Max(
                currentGear - 1, 1);
    }

    public void AutomaticShift(float throttle)
    {
        if (transmission == null ||
            mode != GearMode.Drive)
            return;

        if (engineRPM >= transmission.upshiftRPM &&
            currentGear < GetMaximumGear())
        {
            currentGear++;
        }
        else if (engineRPM <= transmission.downshiftRPM &&
                 currentGear > 1 &&
                 throttle > 0.1f)
        {
            currentGear--;
        }
    }

    private int GetMaximumGear()
    {
        if (transmission == null ||
            transmission.gearRatios == null ||
            transmission.gearRatios.Length == 0)
            return 1;

        return Mathf.Max(
            1,
            Mathf.Min(
                transmission.forwardGears,
                transmission.gearRatios.Length));
    }
}
