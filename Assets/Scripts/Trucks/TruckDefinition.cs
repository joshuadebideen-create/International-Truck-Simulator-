
using UnityEngine;

[CreateAssetMenu(
    fileName = "NewTruck",
    menuName = "Truck Simulator/Truck Definition"
)]
public class TruckDefinition : ScriptableObject
{
    [Header("Truck Identity")]
    public string truckName = "International Truck";
    public string manufacturer = "International";
    public string modelYear = "2026";

    [Header("Engine")]
    [Min(1)]
    public int horsepower = 500;

    [Min(0)]
    public float torqueNm = 2500f;

    [Header("Performance")]
    [Min(1f)]
    public float maximumSpeedKmh = 120f;

    [Min(100f)]
    public float vehicleMassKg = 8000f;

    [Header("Transmission")]
    [Min(1)]
    public int forwardGears = 12;

    [Min(0.1f)]
    public float finalDriveRatio = 3.55f;
}
