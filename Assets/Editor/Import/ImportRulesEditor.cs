using UnityEditor;
using UnityEngine;

namespace DarknessInside.Editor.Import
{
    [CustomEditor(typeof(ImportRules))]
    public sealed class ImportRulesEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox(
                "Новые файлы получают эти настройки автоматически. Кнопка ниже применяет правила ко всем существующим " +
                "текстурам и звукам (ручные правки в совпавших папках будут перезаписаны).",
                MessageType.Info);

            if (GUILayout.Button("Применить ко всем существующим ассетам"))
            {
                EditorApplication.delayCall += () => ImportRulesApplier.ApplyToAll((ImportRules)target);
            }
        }
    }

    public static class ImportRulesApplier
    {
        [MenuItem("Darkness Inside/Import/Apply Import Rules To All Assets", priority = 50)]
        private static void ApplyFromMenu()
        {
            ImportRules rules = ImportRules.Load();
            if (rules == null)
            {
                Debug.LogError("[Import] Не найден " + ImportRules.AssetPath);
                return;
            }

            ApplyToAll(rules);
        }

        /// <summary>Применяет правила ко всем текстурам и аудио в Assets. Возвращает число переимпортированных файлов.</summary>
        public static int ApplyToAll(ImportRules rules)
        {
            int changed = 0;
            AssetDatabase.StartAssetEditing();
            try
            {
                foreach (string guid in AssetDatabase.FindAssets("t:Texture2D", new[] { "Assets" }))
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    ImportRules.TextureRule rule = rules.FindTextureRule(path);
                    if (rule != null && AssetImporter.GetAtPath(path) is TextureImporter importer && ImportRules.Apply(importer, rule))
                    {
                        importer.SaveAndReimport();
                        changed++;
                    }
                }

                foreach (string guid in AssetDatabase.FindAssets("t:AudioClip", new[] { "Assets" }))
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    ImportRules.AudioRule rule = rules.FindAudioRule(path);
                    if (rule != null && AssetImporter.GetAtPath(path) is AudioImporter importer && ImportRules.Apply(importer, rule))
                    {
                        importer.SaveAndReimport();
                        changed++;
                    }
                }
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
            }

            Debug.Log($"[Import] Правила применены, переимпортировано файлов: {changed}");
            return changed;
        }
    }
}
