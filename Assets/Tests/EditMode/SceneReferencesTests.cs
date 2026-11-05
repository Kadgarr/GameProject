using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DarknessInside.Tests
{
    /// <summary>Обязательные ссылки на ассеты в сценах не потеряны (например, адрес политики у кнопок-ссылок).</summary>
    public class SceneReferencesTests
    {
        private SceneSetup[] _setup;

        [OneTimeSetUp]
        public void Save() => _setup = EditorSceneManager.GetSceneManagerSetup();

        [OneTimeTearDown]
        public void Restore()
        {
            if (_setup != null && _setup.Length > 0)
            {
                EditorSceneManager.RestoreSceneManagerSetup(_setup);
            }
        }

        [Test]
        public void ProjectTimeScale_IsOne()
        {
            var timeManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TimeManager.asset")[0]);
            Assert.That(timeManager.FindProperty("m_TimeScale").floatValue, Is.EqualTo(1f),
                "Project Settings → Time → Time Scale должен быть 1, иначе игра стартует на паузе.");
        }

        [Test]
        public void PrivacyPolicyLinks_HaveExternalLinksAsset()
        {
            Scene scene = EditorSceneManager.OpenScene("Assets/Scenes/SceneMenu.unity", OpenSceneMode.Single);
            var missing = new List<string>();
            int found = 0;
            foreach (GameObject root in scene.GetRootGameObjects())
            {
                foreach (DarknessInside.UI.Menu.PrivacyPolicyLink link in root.GetComponentsInChildren<DarknessInside.UI.Menu.PrivacyPolicyLink>(true))
                {
                    found++;
                    if (new SerializedObject(link).FindProperty("links").objectReferenceValue == null)
                    {
                        missing.Add(AnimationUtility.CalculateTransformPath(link.transform, null));
                    }
                }
            }

            Assert.That(found, Is.GreaterThan(0), "В меню нет ссылок на политику конфиденциальности.");
            Assert.That(missing, Is.Empty, "Не задан ExternalLinks: " + string.Join(", ", missing));
        }
    }
}
