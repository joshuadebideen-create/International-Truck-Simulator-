
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public static class FirstPlayableSceneGenerator
{
    private const string SceneFolder = "Assets/Scenes";
    private const string ScenePath =
        "Assets/Scenes/FirstPlayableScene.unity";

    [MenuItem("Tools/International Truck Simulator/Create First Playable Scene")]
    public static void CreateScene()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogError("Stop Play mode before creating the scene.");
            return;
        }

        if (!AssetDatabase.IsValidFolder(SceneFolder))
        {
            AssetDatabase.CreateFolder("Assets", "Scenes");
        }

        if (System.IO.File.Exists(ScenePath))
        {
            bool replace = EditorUtility.DisplayDialog(
                "Scene already exists",
                "FirstPlayableScene already exists. Replace it?",
                "Replace Scene",
                "Cancel");

            if (!replace)
                return;
        }

        Scene scene = EditorSceneManager.NewScene(
            NewSceneSetup.EmptyScene,
            NewSceneMode.Single);

        Material roadMaterial = CreateMaterial(
            "RoadMaterial", new Color(0.22f, 0.23f, 0.24f));

        Material chassisMaterial = CreateMaterial(
            "TruckRed", new Color(0.65f, 0.04f, 0.025f));

        Material cabMaterial = CreateMaterial(
            "CabMaterial", new Color(0.78f, 0.05f, 0.025f));

        Material glassMaterial = CreateMaterial(
            "GlassMaterial", new Color(0.12f, 0.22f, 0.28f));

        Material tyreMaterial = CreateMaterial(
            "TyreMaterial", new Color(0.06f, 0.06f, 0.06f));

        GameObject ground = GameObject.CreatePrimitive(
            PrimitiveType.Cube);

        ground.name = "Test Ground";
        ground.transform.position = new Vector3(0f, -0.25f, 0f);
        ground.transform.localScale = new Vector3(100f, 0.5f, 300f);
        ground.GetComponent<Renderer>().sharedMaterial = roadMaterial;

        GameObject truck = new GameObject("International Prototype Truck");
        truck.transform.position = new Vector3(0f, 1.2f, 0f);

        Rigidbody body = truck.AddComponent<Rigidbody>();
        body.mass = 8000f;
        body.linearDamping = 0.02f;
        body.angularDamping = 0.5f;
        body.interpolation = RigidbodyInterpolation.Interpolate;
        body.constraints = RigidbodyConstraints.FreezeRotationZ;

        BoxCollider chassisCollider = truck.AddComponent<BoxCollider>();
        chassisCollider.size = new Vector3(2.4f, 1f, 5.5f);
        chassisCollider.center = new Vector3(0f, 0f, -0.4f);

        CreateCube("Chassis", truck.transform,
            new Vector3(0f, 0f, -0.4f),
            new Vector3(2.4f, 0.8f, 5.5f), chassisMaterial);

        CreateCube("Cab", truck.transform,
            new Vector3(0f, 0.95f, 1.45f),
            new Vector3(2.2f, 1.9f, 1.9f), cabMaterial);

        CreateCube("Windshield", truck.transform,
            new Vector3(0f, 1.25f, 2.42f),
            new Vector3(1.65f, 0.85f, 0.08f), glassMaterial);

        TruckController controller =
            truck.AddComponent<TruckController>();

        WheelCollider[] driveWheels = new WheelCollider[2];
        WheelCollider[] steeringWheels = new WheelCollider[2];
        WheelCollider[] brakeWheels = new WheelCollider[4];

        float[] axleZ = { 1.65f, -1.9f, -2.65f };

        for (int i = 0; i < 4; i++)
        {
            bool front = i < 2;
            float x = (i % 2 == 0) ? -1.15f : 1.15f;
            float z = front ? axleZ[0] : axleZ[1];
            if (!front && i == 3) z = axleZ[2];

            GameObject wheel = GameObject.CreatePrimitive(
                PrimitiveType.Cylinder);

            wheel.name = front ? "Front Tyre " + i : "Rear Tyre " + i;
            wheel.transform.SetParent(truck.transform, false);
            wheel.transform.localPosition = new Vector3(x, -0.45f, z);
            wheel.transform.localRotation =
                Quaternion.Euler(0f, 0f, 90f);
            wheel.transform.localScale = new Vector3(0.85f, 0.24f, 0.85f);
            wheel.GetComponent<Renderer>().sharedMaterial = tyreMaterial;
            Object.DestroyImmediate(wheel.GetComponent<Collider>());

            GameObject wheelObject = new GameObject(
                "Wheel Collider " + i);

            wheelObject.transform.SetParent(truck.transform, false);
            wheelObject.transform.localPosition =
                new Vector3(x, -0.45f, z);

            WheelCollider wheelCollider =
                wheelObject.AddComponent<WheelCollider>();

            wheelCollider.radius = 0.42f;
            wheelCollider.suspensionDistance = 0.2f;
            wheelCollider.mass = 80f;

            brakeWheels[i] = wheelCollider;

            if (front)
            {
                steeringWheels[i] = wheelCollider;
            }
            else
            {
                driveWheels[i - 2] = wheelCollider;
            }
        }

        SerializedObject serializedController =
            new SerializedObject(controller);

        serializedController.FindProperty("driveWheels")
            .arraySize = driveWheels.Length;

        SerializedProperty driveProperty =
            serializedController.FindProperty("driveWheels");

        for (int i = 0; i < driveWheels.Length; i++)
            driveProperty.GetArrayElementAtIndex(i).objectReferenceValue =
                driveWheels[i];

        SerializedProperty steeringProperty =
            serializedController.FindProperty("steeringWheels");

        steeringProperty.arraySize = steeringWheels.Length;

        for (int i = 0; i < steeringWheels.Length; i++)
            steeringProperty.GetArrayElementAtIndex(i).objectReferenceValue =
                steeringWheels[i];

        SerializedProperty brakeProperty =
            serializedController.FindProperty("brakeWheels");

        brakeProperty.arraySize = brakeWheels.Length;

        for (int i = 0; i < brakeWheels.Length; i++)
            brakeProperty.GetArrayElementAtIndex(i).objectReferenceValue =
                brakeWheels[i];

        serializedController.ApplyModifiedProperties();

        GameObject cameraObject = new GameObject("Chase Camera");
        Camera camera = cameraObject.AddComponent<Camera>();
        camera.tag = "MainCamera";
        camera.fieldOfView = 65f;
        cameraObject.AddComponent<AudioListener>();

        cameraObject.transform.position =
            truck.transform.position + new Vector3(0f, 5f, -9f);

        cameraObject.transform.LookAt(
            truck.transform.position + new Vector3(0f, 1f, 1f));

        GameObject lightObject = new GameObject("Sun");
        Light sun = lightObject.AddComponent<Light>();
        sun.type = LightType.Directional;
        sun.intensity = 1.2f;
        lightObject.transform.rotation =
            Quaternion.Euler(50f, -30f, 0f);

        RenderSettings.ambientLight =
            new Color(0.55f, 0.55f, 0.55f);

        EditorSceneManager.SaveScene(scene, ScenePath);
        AssetDatabase.SaveAssets();

        EditorBuildSettings.scenes =
            new EditorBuildSettingsScene[]
            {
                new EditorBuildSettingsScene(ScenePath, true)
            };

        Debug.Log("First Playable Scene created at " + ScenePath);
        EditorUtility.DisplayDialog(
            "Scene Created",
            "The prototype scene was created and added to Build Settings.",
            "OK");
    }

    private static GameObject CreateCube(
        string objectName,
        Transform parent,
        Vector3 position,
        Vector3 scale,
        Material material)
    {
        GameObject cube = GameObject.CreatePrimitive(
            PrimitiveType.Cube);

        cube.name = objectName;
        cube.transform.SetParent(parent, false);
        cube.transform.localPosition = position;
        cube.transform.localScale = scale;
        cube.GetComponent<Renderer>().sharedMaterial = material;
        Object.DestroyImmediate(cube.GetComponent<Collider>());

        return cube;
    }

    private static Material CreateMaterial(
        string materialName,
        Color color)
    {
        Material material = new Material(
            Shader.Find("Universal Render Pipeline/Lit"));

        if (material.shader == null)
            material.shader = Shader.Find("Standard");

        material.name = materialName;
        material.color = color;
        return material;
    }
}
