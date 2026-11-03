using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DarknessInside.Tests
{
    /// <summary>Проверки целостности проекта без запуска игры.</summary>
    public class ProjectIntegrityTests
    {
        private SceneSetup[] _sceneSetup;

        [OneTimeSetUp]
        public void SaveOpenScenes() => _sceneSetup = EditorSceneManager.GetSceneManagerSetup();

        [OneTimeTearDown]
        public void RestoreOpenScenes()
        {
            if (_sceneSetup != null && _sceneSetup.Length > 0)
            {
                EditorSceneManager.RestoreSceneManagerSetup(_sceneSetup);
            }
        }

        private static IEnumerable<string> BuildScenes()
        {
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                {
                    yield return scene.path;
                }
            }
        }

        [Test]
        public void BuildSettings_HasScenes()
        {
            Assert.That(EditorBuildSettings.scenes.Length, Is.GreaterThan(0));
        }

        [TestCaseSource(nameof(BuildScenes))]
        public void Scene_HasNoMissingScripts(string scenePath)
        {
            Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            var broken = new List<string>();

            foreach (GameObject root in scene.GetRootGameObjects())
            {
                foreach (Transform t in root.GetComponentsInChildren<Transform>(true))
                {
                    if (GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(t.gameObject) > 0)
                    {
                        broken.Add(AnimationUtility.CalculateTransformPath(t, null));
                    }
                }
            }

            Assert.That(broken, Is.Empty, $"Missing Script в {scenePath}: {string.Join(", ", broken)}");
        }

        [Test]
        public void Prefabs_HaveNoMissingScripts()
        {
            var broken = new List<string>();

            foreach (string guid in AssetDatabase.FindAssets("t:Prefab", new[] { "Assets" }))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                foreach (Transform t in prefab.GetComponentsInChildren<Transform>(true))
                {
                    if (GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(t.gameObject) > 0)
                    {
                        broken.Add(path);
                        break;
                    }
                }
            }

            Assert.That(broken, Is.Empty, "Префабы с Missing Script: " + string.Join(", ", broken));
        }
    }
}
