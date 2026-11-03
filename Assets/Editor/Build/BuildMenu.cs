using UnityEditor;
using UnityEngine;

namespace DarknessInside.Editor.Build
{
    /// <summary>Пункты меню для быстрой сборки по готовым конфигам.</summary>
    public static class BuildMenu
    {
        private const string TestConfigPath = "Assets/Settings/Build/AndroidTest.asset";
        private const string ReleaseConfigPath = "Assets/Settings/Build/AndroidRelease.asset";

        [MenuItem("Darkness Inside/Build/Android Test (APK)", priority = 0)]
        private static void BuildTest() => BuildFrom(TestConfigPath);

        [MenuItem("Darkness Inside/Build/Android Release (AAB)", priority = 1)]
        private static void BuildRelease() => BuildFrom(ReleaseConfigPath);

        [MenuItem("Darkness Inside/Build/Select Build Configs", priority = 20)]
        private static void SelectConfigs()
        {
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<AndroidBuildConfig>(TestConfigPath);
        }

        private static void BuildFrom(string path)
        {
            var config = AssetDatabase.LoadAssetAtPath<AndroidBuildConfig>(path);
            if (config == null)
            {
                Debug.LogError($"[Build] Не найден конфиг сборки: {path}");
                return;
            }

            config.Build();
        }
    }
}
