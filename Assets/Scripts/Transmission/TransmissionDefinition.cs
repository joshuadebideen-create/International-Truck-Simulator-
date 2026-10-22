
using UnityEngine;

[CreateAssetMenu(
    fileName = "NewTransmission",
    menuName = "Truck Simulator/Transmission Definition"
)]
public class TransmissionDefinition : ScriptableObject
{
    [Header("Transmission")]
    public string transmissionName = "12-Speed Automatic";

    [Min(1)]
    public int forwardGears = 12;

    public bool hasReverse = true;

    [Header("Forward Gear Ratios")]
    public float[] gearRatios =
    {
        14.94f,
        11.73f,
        9.04f,
        7.09f,
        5.57f,
        4.38f,
        3.44f,
        2.70f,
        2.12f,
        1.66f,
        1.30f,
        1.00f
    };

    [Header("Differential")]
    [Min(0.1f)]
    public float finalDriveRatio = 3.55f;

    [Header("Automatic Shifting")]
    [Min(500f)]
    public float upshiftRPM = 1800f;

    [Min(500f)]
    public float downshiftRPM = 1000f;

    public float GetGearRatio(int gear)
    {
        if (gear < 1 || gear > gearRatios.Length)
            return 0f;

        return gearRatios[gear - 1];
    }
}
