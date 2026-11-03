using UnityEditor;
using UnityEngine;

namespace DarknessInside.Editor.Build
{
    [CustomEditor(typeof(AndroidBuildConfig))]
    public sealed class AndroidBuildConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var config = (AndroidBuildConfig)target;
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Файл сборки", config.FullOutputPath);

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Применить настройки"))
                {
                    config.Apply();
                }

                if (GUILayout.Button("Собрать"))
                {
                    // Сборка вне OnGUI, чтобы не ломать layout инспектора.
                    EditorApplication.delayCall += () => config.Build();
                }
            }
        }
    }
}
