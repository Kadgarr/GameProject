using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace DarknessInside.Tests
{
    /// <summary>
    /// Каждая сцена из Build Settings загружается и работает 2 секунды без ошибок в консоли.
    /// Любой Debug.LogError или исключение за это время валит тест (поведение LogAssert по умолчанию).
    /// </summary>
    public class SceneLoadTests
    {
        private const float RunSeconds = 2f;

        private static IEnumerable<int> SceneIndices()
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                yield return i;
            }
        }

        [UnityTest]
        public IEnumerator Scene_LoadsAndRunsWithoutErrors([ValueSource(nameof(SceneIndices))] int buildIndex)
        {
            AsyncOperation load = SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Single);
            yield return load;

            Assert.That(SceneManager.GetActiveScene().buildIndex, Is.EqualTo(buildIndex));

            yield return new WaitForSecondsRealtime(RunSeconds);
        }
    }
}
