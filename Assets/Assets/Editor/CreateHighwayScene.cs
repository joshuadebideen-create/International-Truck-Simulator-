
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class CreateHighwayScene
{
    [MenuItem("International Truck Simulator/Create Starter Highway")]
    public static void Create()
    {
        var scene = EditorSceneManager.NewScene(
            NewSceneSetup.EmptyScene,
            NewSceneMode.Single
        );

        RenderSettings.ambientLight = new Color(0.55f, 0.58f, 0.62f);
        RenderSettings.fog = true;
        RenderSettings.fogColor = new Color(0.65f, 0.75f, 0.85f);
        RenderSettings.fogDensity = 0.002f;

        var sun = new GameObject("Sun");
        var light = sun.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.2f;
        sun.transform.rotation = Quaternion.Euler(45f, -30f, 0f);

        MakeCube("Highway", new Vector3(0, -0.15f, 100),
            new Vector3(12, 0.3f, 220),
            new Color(0.16f, 0.16f, 0.17f));

        MakeCube("Left Shoulder", new Vector3(-8, -0.2f, 100),
            new Vector3(4, 0.2f, 220), new Color(0.35f, 0.35f, 0.3f));

        MakeCube("Right Shoulder", new Vector3(8, -0.2f, 100),
            new Vector3(4, 0.2f, 220), new Color(0.35f, 0.35f, 0.3f));

        for (int z = 0; z < 220; z += 10)
        {
            MakeCube("Center Line", new Vector3(0, 0.02f, z + 5),
                new Vector3(0.15f, 0.03f, 4),
                new Color(1f, 0.8f, 0.1f));
        }

        var truck = new GameObject("Starter Truck");
        truck.transform.position = new Vector3(0, 0.9f, 8);
        truck.AddComponent<Rigidbody>().mass = 8000f;

        MakeChild(truck.transform, "Cab",
            new Vector3(0, 0.7f, 1.5f),
            new Vector3(2.4f, 2.3f, 2.5f),
            new Color(0.65f, 0.05f, 0.04f));

        MakeChild(truck.transform, "Trailer",
            new Vector3(0, 1.1f, -5),
            new Vector3(2.5f, 2.8f, 8),
            new Color(0.8f, 0.8f, 0.78f));

        var cameraObject = new GameObject("Truck Chase Camera");
        var cam = cameraObject.AddComponent<Camera>();
        cam.fieldOfView = 65f;
        cameraObject.AddComponent<AudioListener>();
        cameraObject.transform.position = new Vector3(0, 5, 1);
        cameraObject.transform.rotation = Quaternion.Euler(12f, 0, 0);

        var ground = MakeCube("Terrain",
            new Vector3(0, -0.5f, 100),
            new Vector3(100, 0.2f, 240),
            new Color(0.25f, 0.42f, 0.2f));

        ground.transform.SetSiblingIndex(0);

        string folder = "Assets/Scenes";
        if (!AssetDatabase.IsValidFolder(folder))
            AssetDatabase.CreateFolder("Assets", "Scenes");

        EditorSceneManager.SaveScene(scene,
            "Assets/Scenes/StarterHighway.unity");

        AssetDatabase.SaveAssets();
        Debug.Log("Starter Highway created. Open Assets/Scenes/StarterHighway.unity");
    }

    static GameObject MakeCube(string name, Vector3 position,
        Vector3 scale, Color color)
    {
        var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.name = name;
        obj.transform.position = position;
        obj.transform.localScale = scale;
        obj.GetComponent<Renderer>().sharedMaterial = MakeMaterial(color);
        return obj;
    }

    static void MakeChild(Transform parent, string name,
        Vector3 localPosition, Vector3 scale, Color color)
    {
        var obj = MakeCube(name, parent.position + localPosition, scale, color);
        obj.transform.SetParent(parent);
        obj.transform.localPosition = localPosition;
    }

    static Material MakeMaterial(Color color)
    {
        var shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null) shader = Shader.Find("Standard");

        var material = new Material(shader);
        material.color = color;
        return material;
    }
}
