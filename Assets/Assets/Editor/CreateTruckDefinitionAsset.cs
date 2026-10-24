
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

public static class CreateTruckDefinitionAsset
{
    private const string Folder =
        "Assets/ScriptableObjects/Trucks";

    private const string AssetPath =
        Folder + "/International_2026.asset";

    [MenuItem("Truck Simulator/Create International Truck Asset")]
    public static void CreateAsset()
    {
        if (!Directory.Exists(Folder))
            Directory.CreateDirectory(Folder);

        if (AssetDatabase.LoadAssetAtPath<TruckDefinition>(AssetPath) != null)
        {
            Debug.LogWarning(
                "Truck asset already exists: " + AssetPath);
            return;
        }

        TruckDefinition truck =
            ScriptableObject.CreateInstance<TruckDefinition>();

        truck.truckName = "International Truck";
        truck.manufacturer = "International";
        truck.modelYear = "2026";
        truck.horsepower = 500;
        truck.torqueNm = 2500f;
        truck.maximumSpeedKmh = 120f;
        truck.vehicleMassKg = 8000f;
        truck.forwardGears = 12;
        truck.finalDriveRatio = 3.55f;

        AssetDatabase.CreateAsset(truck, AssetPath);
        AssetDatabase.SaveAssets();

        Selection.activeObject = truck;
        EditorGUIUtility.PingObject(truck);

        Debug.Log("Created truck asset: " + AssetPath);
    }
}
#endif
