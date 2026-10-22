
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ConnectFirstPlayableScene
{
    private const string ScenePath =
        "Assets/Scenes/FirstPlayableScene.unity";

    [MenuItem("Tools/International Truck Simulator/Connect First Playable Scene")]
    public static void ConnectScene()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogError("Stop Play mode before connecting the scene.");
            return;
        }

        if (!System.IO.File.Exists(ScenePath))
        {
            EditorUtility.DisplayDialog(
                "Scene not found",
                "Create First Playable Scene first, then run this tool.",
                "OK");
            return;
        }

        Scene scene = EditorSceneManager.OpenScene(
            ScenePath, OpenSceneMode.Single);

        GameObject truck =
            GameObject.Find("International Prototype Truck");

        Camera camera = Camera.main;

        if (truck == null || camera == null)
        {
            EditorUtility.DisplayDialog(
                "Missing scene objects",
                "The prototype truck or Main Camera was not found. " +
                "Create the scene first and check that it contains both.",
                "OK");
            return;
        }

        TruckController controller =
            truck.GetComponent<TruckController>();

        if (controller == null)
        {
            EditorUtility.DisplayDialog(
                "Missing truck controller",
                "TruckController was not found on the prototype truck.",
                "OK");
            return;
        }

        TruckChaseCamera follow =
            camera.GetComponent<TruckChaseCamera>();

        if (follow == null)
            follow = camera.gameObject.AddComponent<TruckChaseCamera>();

        follow.SetTarget(truck.transform);

        GameObject uiBuilderObject =
            new GameObject("Truck Mobile UI Setup");

        TruckMobileUIBuilder uiBuilder =
            uiBuilderObject.AddComponent<TruckMobileUIBuilder>();

        uiBuilder.Build(controller);

        EditorSceneManager.MarkSceneDirty(scene);

        if (!EditorSceneManager.SaveScene(scene, ScenePath))
        {
            Debug.LogError("Could not save the connected scene.");
            return;
        }

        EditorUtility.DisplayDialog(
            "Scene connected",
            "Camera follow and mobile controls have been added.",
            "OK");
    }
}
