using DarknessInside.Core.Levels;
using DarknessInside.Core.Save;
using NUnit.Framework;
using UnityEngine;

namespace DarknessInside.Tests
{
    public sealed class SaveServiceTests
    {
        private sealed class MemoryStorage : ISaveStorage
        {
            public string Json;
            public int Writes;

            public bool TryLoad(out SaveData data)
            {
                data = string.IsNullOrEmpty(Json) ? null : JsonUtility.FromJson<SaveData>(Json);
                return data != null;
            }

            public void Save(SaveData data)
            {
                Json = JsonUtility.ToJson(data);
                Writes++;
            }

            public void Delete()
            {
                Json = null;
            }
        }

        [Test]
        public void NewSave_HasDefaults()
        {
            var service = new SaveService(new MemoryStorage());

            Assert.AreEqual(0, service.LastCompletedLevel);
            Assert.IsFalse(service.HasProgress);
            Assert.IsTrue(service.MusicOn);
            Assert.IsTrue(service.SoundOn);
        }

        [Test]
        public void Progress_OnlyGrows_AndSurvivesReload()
        {
            var storage = new MemoryStorage();
            var service = new SaveService(storage);
            int raised = 0;
            service.ProgressChanged += level => raised++;

            service.MarkLevelCompleted(3);
            service.MarkLevelCompleted(2);

            Assert.AreEqual(3, service.LastCompletedLevel);
            Assert.AreEqual(1, raised);
            Assert.AreEqual(3, new SaveService(storage).LastCompletedLevel);
        }

        [Test]
        public void Settings_WriteOnlyOnChange()
        {
            var storage = new MemoryStorage();
            var service = new SaveService(storage);

            service.SetMusicOn(true);
            Assert.AreEqual(0, storage.Writes);

            service.SetMusicOn(false);
            service.SetSoundOn(false);
            Assert.AreEqual(2, storage.Writes);

            var reloaded = new SaveService(storage);
            Assert.IsFalse(reloaded.MusicOn);
            Assert.IsFalse(reloaded.SoundOn);
        }

        [Test]
        public void CorruptedFile_StartsFresh()
        {
            string path = System.IO.Path.Combine(Application.temporaryCachePath, "corrupted_save.json");
            System.IO.File.WriteAllText(path, "{\"lastCompletedLevel\":");
            try
            {
                var storage = new JsonFileSaveStorage(path);
                var service = new SaveService(storage);

                Assert.AreEqual(0, service.LastCompletedLevel);
                service.MarkLevelCompleted(2);
                Assert.AreEqual(2, new SaveService(new JsonFileSaveStorage(path)).LastCompletedLevel);
            }
            finally
            {
                System.IO.File.Delete(path);
            }
        }

        [Test]
        public void LevelDatabase_MatchesBuildSettings()
        {
            var database = UnityEditor.AssetDatabase.LoadAssetAtPath<LevelDatabase>("Assets/Settings/Levels/LevelDatabase.asset");
            Assert.IsNotNull(database, "LevelDatabase asset is missing");
            Assert.Greater(database.Count, 0);

            for (int level = 1; level <= database.Count; level++)
            {
                string scene = database.GetSceneName(level);
                Assert.IsTrue(Application.CanStreamedLevelBeLoaded(scene) || IsInBuild(scene), "Level " + level + " scene '" + scene + "' is not in Build Settings");
                Assert.AreEqual(level, database.GetLevelNumber(scene));
            }

            Assert.AreEqual(database.EndScene, database.GetNextScene(database.Count));
            Assert.IsTrue(IsInBuild(database.MenuScene) && IsInBuild(database.EndScene));
        }

        private static bool IsInBuild(string sceneName)
        {
            foreach (var scene in UnityEditor.EditorBuildSettings.scenes)
            {
                if (scene.enabled && System.IO.Path.GetFileNameWithoutExtension(scene.path) == sceneName)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
