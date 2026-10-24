
using System;
using UnityEngine;

[Serializable]
public class TruckFleetEntry
{
    public string truckName;
    public string manufacturer;
    public string modelYear;
    public int horsepower;
    public float torqueNm;
    public float maximumSpeedKmh;
    public float vehicleMassKg;
    public int forwardGears;
    public float finalDriveRatio;
}

[CreateAssetMenu(
    fileName = "TruckFleetCatalog",
    menuName = "Truck Simulator/Truck Fleet Catalog")]
public class TruckFleetCatalog : ScriptableObject
{
    public TruckFleetEntry[] trucks =
    {
        new TruckFleetEntry
        {
            truckName = "Mercedes-Benz LS 1935",
            manufacturer = "Mercedes-Benz",
            modelYear = "Classic",
            horsepower = 350,
            torqueNm = 1450,
            maximumSpeedKmh = 100,
            vehicleMassKg = 7500,
            forwardGears = 16,
            finalDriveRatio = 3.7f
        },
        new TruckFleetEntry
        {
            truckName = "Mercedes-Benz LS 1634",
            manufacturer = "Mercedes-Benz",
            modelYear = "Classic",
            horsepower = 340,
            torqueNm = 1350,
            maximumSpeedKmh = 100,
            vehicleMassKg = 7500,
            forwardGears = 16,
            finalDriveRatio = 3.7f
        }
    };
}
