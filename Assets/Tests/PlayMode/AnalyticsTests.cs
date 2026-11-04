using System.Collections;
using System.Collections.Generic;
using DarknessInside.Analytics;
using DarknessInside.Core.Save;
using DarknessInside.UI.Menu;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace DarknessInside.Tests
{
    /// <summary>
    /// Аналитика: события уровня доходят до сервиса с номером уровня, попыткой и причиной смерти;
    /// окно согласия показывается один раз и сохраняет ответ.
    /// </summary>
    public class AnalyticsTests
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

        private sealed class RecordingService : IAnalyticsService
        {
            public readonly List<string> Events = new List<string>();

            public void SetEnabled(bool enabled) { }

            public void LevelStarted(int level, int attempt) => Events.Add("start " + level + " " + attempt);

            public void LevelCompleted(int level, int seconds, int deaths) => Events.Add("complete " + level + " " + deaths);

            public void PlayerDied(int level, string cause, int seconds) => Events.Add("death " + level + " " + cause);

            public void LevelQuit(int level, int seconds) => Events.Add("quit " + level);

            public void RiddleAttempted(int level, bool success) => Events.Add("riddle " + level + " " + success);

            public void ItemCollected(int level, string itemId) => Events.Add("item " + level + " " + itemId);

            public void HintOpened(int level) => Events.Add("hint " + level);

            public void SettingChanged(string setting, string value) => Events.Add("setting " + setting + " " + value);
        }

        private RecordingService _recorder;

        [SetUp]
        public void UseMemorySave()
        {
            SaveService.SetInstance(new SaveService(new MemoryStorage()));
            _recorder = new RecordingService();
        }

        [TearDown]
        public void Restore()
        {
            if (AnalyticsReporter.Instance != null)
            {
                AnalyticsReporter.Instance.Unregister(_recorder);
            }

            SaveService.SetInstance(null);
        }

        [UnityTest]
        public IEnumerator Death_IsReportedWithCause_AndRestartCountsAttempt()
        {
            Assert.That(AnalyticsReporter.Instance, Is.Not.Null, "AnalyticsReporter нет в Core.");
            AnalyticsReporter.Instance.Register(_recorder);

            yield return SceneManager.LoadSceneAsync("Level_8");
            yield return null;
            Assert.That(_recorder.Events, Does.Contain("start 8 1"));

            Object.FindFirstObjectByType<DeadPlayer>().Kill(0.1f, "thorns");
            Assert.That(_recorder.Events, Does.Contain("death 8 thorns"));

            yield return new WaitForSeconds(0.5f);
            Assert.That(_recorder.Events, Does.Contain("start 8 2"), "после смерти — вторая попытка того же уровня");
        }

        [UnityTest]
        public IEnumerator Settings_AreReported()
        {
            AnalyticsReporter.Instance.Register(_recorder);
            yield return null;

            SaveService.Instance.SetMusicOn(false);
            Assert.That(_recorder.Events, Does.Contain("setting music off"));
        }

        [UnityTest]
        public IEnumerator ConsentPanel_ShowsOnce_AndSavesAnswer()
        {
            yield return SceneManager.LoadSceneAsync("SceneMenu");
            yield return null;

            var panel = Object.FindFirstObjectByType<AnalyticsConsentPanel>();
            Assert.That(panel, Is.Not.Null);
            Assert.That(panel.GetComponent<Canvas>().enabled, Is.True, "без ответа окно видно");

            panel.Decline();
            Assert.That(SaveService.Instance.AnalyticsConsent, Is.EqualTo(ConsentState.Denied));
            Assert.That(panel.GetComponent<Canvas>().enabled, Is.False);

            yield return SceneManager.LoadSceneAsync("SceneMenu");
            yield return null;
            panel = Object.FindFirstObjectByType<AnalyticsConsentPanel>();
            Assert.That(panel.GetComponent<Canvas>().enabled, Is.False, "после ответа окно больше не показывается");
        }
    }
}
