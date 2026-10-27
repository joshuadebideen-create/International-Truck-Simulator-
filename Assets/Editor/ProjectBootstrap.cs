
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InternationalTruckSimulator.Editor
{
    public static class ProjectBootstrap
    {
        public static void PrepareAndroidBuild()
        {
            const string sceneFolder = "Assets/Scenes";
            const string scenePath =
                "Assets/Scenes/InternationalTruckSimulator.unity";
            const string apkPath =
                "Build/InternationalTruckSimulator.apk";

            // Ensure the scenes folder exists.
            if (!AssetDatabase.IsValidFolder(sceneFolder))
            {
                AssetDatabase.CreateFolder("Assets", "Scenes");
            }

            // Keep existing enabled build scenes.
            var scenes = EditorBuildSettings.scenes
              const string starterScenePath =
    "Assets/Scenes/StarterHighway.unity";

if (!File.Exists(starterScenePath))
{
    CreateHighwayScene.Create();
    AssetDatabase.Refresh();
}  
                .Where(scene => scene.enabled &&
                                !string.IsNullOrEmpty(scene.path) &&
                                File.Exists(scene.path))
                .Select(scene => scene.path)
                .ToList();scenes.Insert(0, starterScenePath);

            // If Build Settings has no scenes, find an existing scene.
            if (scenes.Count == 0)
            {
                string[] sceneGuids =
                    AssetDatabase.FindAssets("t:Scene", new[] { "Assets" });

                string[] existingScenes = sceneGuids
                    .Select(AssetDatabase.GUIDToAssetPath)
                    .Where(path => path.EndsWith(
                        ".unity",
                        StringComparison.OrdinalIgnoreCase))
                    .ToArray();

                string chosenScene = existingScenes.FirstOrDefault(
                    path => Path.GetFileNameWithoutExtension(path)
                        .Equals(
                            "InternationalTruckSimulator",
                            StringComparison.OrdinalIgnoreCase));

                if (string.IsNullOrEmpty(chosenScene))
                {
                    chosenScene = existingScenes.FirstOrDefault();
                }

                // Generate the highway scene if no scene exists.
if (string.IsNullOrEmpty(chosenScene))
{
    CreateHighwayScene.Create();

    chosenScene = "Assets/Scenes/StarterHighway.unity";

    AssetDatabase.Refresh();

    Debug.Log(
        "Created starter highway scene at " + chosenScene);
}

                scenes.Add(chosenScene);
            }

            // Update Build Settings without deleting existing scenes.
            EditorBuildSettings.scenes = scenes
                .Distinct()
                .Select(path => new EditorBuildSettingsScene(path, true))
                .ToArray();

            // Ensure the output directory exists.
            Directory.CreateDirectory("Build");

            // Build the Android APK.
            BuildPlayerOptions options = new BuildPlayerOptions
            {
                scenes = scenes.ToArray(),
                locationPathName = apkPath,
                target = BuildTarget.Android,
                options = BuildOptions.None
            };

            BuildReport report = BuildPipeline.BuildPlayer(options);

            if (report.summary.result != BuildResult.Succeeded)
            {
                throw new Exception(
                    "Android build failed. Result: " +
                    report.summary.result +
                    ". Check the Unity build log for details.");
            }

            Debug.Log(
                "Android APK created successfully: " + apkPath);
        }
    }
}
