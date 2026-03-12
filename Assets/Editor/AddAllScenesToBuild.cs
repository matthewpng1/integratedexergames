using UnityEditor;
using UnityEngine;
using System.Linq;

public static class AddAllScenesToBuild
{
    [MenuItem("Build/Add All Scenes to Build Settings")]
    public static void AddAllScenes()
    {
        // Find all scene asset GUIDs
        var guids = AssetDatabase.FindAssets("t:Scene");
        var scenes = guids
            .Select(g => AssetDatabase.GUIDToAssetPath(g))
            .Where(p => !string.IsNullOrEmpty(p))
            .Distinct()
            .OrderBy(p => p)
            .ToArray();

        var buildScenes = scenes.Select(p => new EditorBuildSettingsScene(p, true)).ToArray();

        EditorBuildSettings.scenes = buildScenes;

        Debug.Log($"Added {buildScenes.Length} scenes to Build Settings.");
    }
}
