using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace DarknessInside.Editor.Analytics
{
    /// <summary>
    /// Репозиторий публичный, поэтому ключи GameAnalytics не хранятся в ассете настроек:
    /// они лежат в UserSettings/GameAnalyticsKeys.json (папка в .gitignore) и подставляются в ассет
    /// только на время сборки Android, сразу после сборки ассет снова очищается.
    /// Если ключи появились в ассете (ввели в окне GameAnalytics), они переносятся в локальный файл
    /// при следующей перезагрузке скриптов. Email аккаунта из ассета тоже убирается.
    /// </summary>
    [InitializeOnLoad]
    public sealed class GameAnalyticsKeyVault : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        private const string SettingsPath = "Assets/Resources/GameAnalytics/Settings.asset";
        private const string KeysPath = "UserSettings/GameAnalyticsKeys.json";
        private const int AndroidPlatform = (int)RuntimePlatform.Android;

        [Serializable]
        private sealed class Keys
        {
            public string gameKey;
            public string secretKey;

            public bool IsValid => !string.IsNullOrEmpty(gameKey) && !string.IsNullOrEmpty(secretKey);
        }

        static GameAnalyticsKeyVault()
        {
            EditorApplication.delayCall += MoveKeysOutOfAsset;
        }

        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            if (report.summary.platform != BuildTarget.Android)
            {
                return;
            }

            Keys keys = LoadKeys();
            if (keys == null || !keys.IsValid)
            {
                Debug.LogWarning("GameAnalytics: нет " + KeysPath + " — билд соберётся без отправки аналитики.");
                return;
            }

            WriteKeys(keys);
            // Успешная сборка очищает ключи в OnPostprocessBuild; неудачная — отложенным вызовом после сборки.
            EditorApplication.delayCall += ClearAfterBuild;
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            ClearKeysInAsset();
        }

        private static void ClearAfterBuild()
        {
            if (BuildPipeline.isBuildingPlayer)
            {
                EditorApplication.delayCall += ClearAfterBuild;
                return;
            }

            ClearKeysInAsset();
        }

        [MenuItem("Darkness Inside/Analytics/Move GameAnalytics Keys To Local File")]
        private static void MoveKeysOutOfAsset()
        {
            SerializedObject settings = LoadSettings();
            if (settings == null || BuildPipeline.isBuildingPlayer)
            {
                return;
            }

            int index = AndroidIndex(settings);
            if (index >= 0)
            {
                var keys = new Keys
                {
                    gameKey = ArrayString(settings, "gameKey", index),
                    secretKey = ArrayString(settings, "secretKey", index),
                };

                if (keys.IsValid)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(KeysPath));
                    File.WriteAllText(KeysPath, JsonUtility.ToJson(keys, true));
                    Debug.Log("GameAnalytics: ключи Android перенесены в " + KeysPath + " (вне git).");
                }
            }

            ClearKeysInAsset();
        }

        private static void ClearKeysInAsset()
        {
            SerializedObject settings = LoadSettings();
            if (settings == null)
            {
                return;
            }

            bool changed = ClearArray(settings, "gameKey") | ClearArray(settings, "secretKey");
            SerializedProperty email = settings.FindProperty("EmailGA");
            if (email != null && !string.IsNullOrEmpty(email.stringValue))
            {
                email.stringValue = string.Empty;
                changed = true;
            }

            if (changed)
            {
                settings.ApplyModifiedPropertiesWithoutUndo();
                AssetDatabase.SaveAssets();
            }
        }

        private static void WriteKeys(Keys keys)
        {
            SerializedObject settings = LoadSettings();
            int index = settings != null ? AndroidIndex(settings) : -1;
            if (index < 0)
            {
                Debug.LogWarning("GameAnalytics: в настройках нет платформы Android.");
                return;
            }

            settings.FindProperty("gameKey").GetArrayElementAtIndex(index).stringValue = keys.gameKey;
            settings.FindProperty("secretKey").GetArrayElementAtIndex(index).stringValue = keys.secretKey;
            settings.ApplyModifiedPropertiesWithoutUndo();
            AssetDatabase.SaveAssets();
        }

        private static Keys LoadKeys()
        {
            return File.Exists(KeysPath) ? JsonUtility.FromJson<Keys>(File.ReadAllText(KeysPath)) : null;
        }

        private static SerializedObject LoadSettings()
        {
            var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(SettingsPath);
            return asset != null ? new SerializedObject(asset) : null;
        }

        private static int AndroidIndex(SerializedObject settings)
        {
            SerializedProperty platforms = settings.FindProperty("Platforms");
            for (int i = 0; platforms != null && i < platforms.arraySize; i++)
            {
                if (platforms.GetArrayElementAtIndex(i).intValue == AndroidPlatform)
                {
                    return i;
                }
            }

            return -1;
        }

        private static string ArrayString(SerializedObject settings, string name, int index)
        {
            SerializedProperty array = settings.FindProperty(name);
            return array != null && index < array.arraySize ? array.GetArrayElementAtIndex(index).stringValue : null;
        }

        private static bool ClearArray(SerializedObject settings, string name)
        {
            SerializedProperty array = settings.FindProperty(name);
            bool changed = false;
            for (int i = 0; array != null && i < array.arraySize; i++)
            {
                SerializedProperty item = array.GetArrayElementAtIndex(i);
                if (!string.IsNullOrEmpty(item.stringValue))
                {
                    item.stringValue = string.Empty;
                    changed = true;
                }
            }

            return changed;
        }
    }
}
