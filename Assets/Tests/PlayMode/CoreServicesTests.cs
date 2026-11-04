using System.Collections;
using DarknessInside.Core.Audio;
using DarknessInside.Core.Localization;
using DarknessInside.Core.Save;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace DarknessInside.Tests
{
    /// <summary>Сервисы Core: одна EventSystem, музыка между сценами, звук и язык из настроек.</summary>
    public sealed class CoreServicesTests
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
        public IEnumerator Core_ExistsOnce_WithSingleEventSystem()
        {
            yield return Load("SceneMenu");
            yield return Load("Level_1");

            Assert.IsNotNull(AudioService.Instance);
            Assert.IsNotNull(LocalizationService.Instance);
            Assert.AreEqual(1, Object.FindObjectsByType<AudioService>(FindObjectsSortMode.None).Length);
            Assert.AreEqual(1, Object.FindObjectsByType<EventSystem>(FindObjectsSortMode.None).Length);
        }

        [UnityTest]
        public IEnumerator LevelMusic_ContinuesBetweenLevels()
        {
            yield return Load("Level_1");
            AudioSource music = MusicSource();
            Assert.IsTrue(music.isPlaying);
            AudioClip clip = music.clip;

            yield return new WaitForSeconds(0.3f);
            yield return Load("Level_2");

            Assert.AreSame(clip, music.clip);
            Assert.IsTrue(music.isPlaying);
            Assert.Greater(music.time, 0.2f, "track restarted instead of continuing");
        }

        [UnityTest]
        public IEnumerator MusicSetting_PausesAndResumes()
        {
            yield return Load("Level_1");
            AudioSource music = MusicSource();

            SaveService.Instance.SetMusicOn(false);
            Assert.IsFalse(music.isPlaying);

            SaveService.Instance.SetMusicOn(true);
            Assert.IsTrue(music.isPlaying);
        }

        [UnityTest]
        public IEnumerator LanguageChange_UpdatesTexts()
        {
            yield return Load("Level_1");
            Text text = Object.FindFirstObjectByType<LocalizedText>().GetComponent<Text>();

            LocalizationService.Instance.SetLanguage("en_US");
            string english = text.text;
            LocalizationService.Instance.SetLanguage("ru_RU");

            Assert.AreNotEqual(english, text.text);
            Assert.AreEqual("ru_RU", SaveService.Instance.Language);
        }

        private static AudioSource MusicSource()
        {
            return AudioService.Instance.GetComponent<AudioSource>();
        }

        private static IEnumerator Load(string scene)
        {
            yield return SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
            yield return null;
        }
    }
}
