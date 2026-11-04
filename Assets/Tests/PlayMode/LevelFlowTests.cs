using System.Collections;
using DarknessInside.Core.Save;
using DarknessInside.Gameplay.Levels;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace DarknessInside.Tests
{
    /// <summary>Смерть → перезапуск, выход → сохранение и следующий уровень.</summary>
    public sealed class LevelFlowTests
    {
        private sealed class MemoryStorage : ISaveStorage
        {
            private SaveData _data;

            public bool TryLoad(out SaveData data)
            {
                data = _data;
                return data != null;
            }

            public void Save(SaveData data)
            {
                _data = data;
            }

            public void Delete()
            {
                _data = null;
            }
        }

        [SetUp]
        public void UseMemorySave()
        {
            SaveService.SetInstance(new SaveService(new MemoryStorage()));
        }

        [TearDown]
        public void RestoreSave()
        {
            SaveService.SetInstance(null);
        }

        [UnityTest]
        public IEnumerator PlayerDeath_RestartsSameLevel()
        {
            yield return LoadScene("Level_8");

            int diedOn = -1;
            System.Action<int> onDied = level => diedOn = level;
            LevelFlow.PlayerDied += onDied;
            try
            {
                Object.FindFirstObjectByType<DeadPlayer>().Kill(0.2f);
                Assert.AreEqual(8, diedOn);

                yield return new WaitForSeconds(0.6f);

                Assert.AreEqual("Level_8", SceneManager.GetActiveScene().name);
                Assert.IsFalse(Object.FindFirstObjectByType<DeadPlayer>().IsDead, "player should be alive after restart");
            }
            finally
            {
                LevelFlow.PlayerDied -= onDied;
            }
        }

        [UnityTest]
        public IEnumerator LevelExit_SavesProgressAndLoadsNextLevel()
        {
            yield return LoadScene("Level_3");

            Object.FindFirstObjectByType<LevelFlow>().CompleteLevel();
            yield return null;
            yield return null;

            Assert.AreEqual(3, SaveService.Instance.LastCompletedLevel);
            Assert.AreEqual("Level_4", SceneManager.GetActiveScene().name);
        }

        [UnityTest]
        public IEnumerator LastLevelExit_LoadsEndScene()
        {
            yield return LoadScene("Level_15");

            Object.FindFirstObjectByType<LevelFlow>().CompleteLevel();
            yield return null;
            yield return null;

            Assert.AreEqual(15, SaveService.Instance.LastCompletedLevel);
            Assert.AreEqual("EndScene", SceneManager.GetActiveScene().name);
        }

        private static IEnumerator LoadScene(string name)
        {
            yield return SceneManager.LoadSceneAsync(name, LoadSceneMode.Single);
            yield return null;
        }
    }
}
