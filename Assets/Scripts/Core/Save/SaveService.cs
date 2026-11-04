using System;
using UnityEngine;

namespace DarknessInside.Core.Save
{
    /// <summary>
    /// Прогресс и настройки игрока. Единственное место, которое читает и пишет сохранение;
    /// остальные узнают об изменениях через события (без опроса и без чтения файлов в каждой сцене).
    /// Файл пишется только при реальном изменении значения.
    /// </summary>
    public sealed class SaveService
    {
        private static SaveService _instance;

        private readonly ISaveStorage _storage;
        private readonly SaveData _data;

        public SaveService(ISaveStorage storage)
        {
            _storage = storage;
            SaveData loaded;
            _data = storage.TryLoad(out loaded) && loaded != null ? loaded : new SaveData();
            Migrate(_data);
        }

        /// <summary>Сервис с сохранением в файл. В Core-сцене (этап 3.4) регистрируется явно.</summary>
        public static SaveService Instance =>
            _instance ?? (_instance = new SaveService(new JsonFileSaveStorage(JsonFileSaveStorage.DefaultPath)));

        /// <summary>Пройден новый уровень. Аргумент — номер последнего пройденного уровня.</summary>
        public event Action<int> ProgressChanged;

        public event Action<bool> MusicChanged;

        public event Action<bool> SoundChanged;

        public event Action<string> LanguageChanged;

        public int LastCompletedLevel => _data.lastCompletedLevel;

        public bool HasProgress => _data.lastCompletedLevel > 0;

        public bool MusicOn => _data.musicOn;

        public bool SoundOn => _data.soundOn;

        public string Language => _data.language;

        /// <summary>Подменить сервис (тесты, Core-сцена).</summary>
        public static void SetInstance(SaveService service)
        {
            _instance = service;
        }

        public void MarkLevelCompleted(int levelNumber)
        {
            if (levelNumber <= _data.lastCompletedLevel)
            {
                return;
            }

            _data.lastCompletedLevel = levelNumber;
            Flush();
            ProgressChanged?.Invoke(levelNumber);
        }

        public void ResetProgress()
        {
            if (_data.lastCompletedLevel == 0)
            {
                return;
            }

            _data.lastCompletedLevel = 0;
            Flush();
            ProgressChanged?.Invoke(0);
        }

        public void SetMusicOn(bool on)
        {
            if (_data.musicOn == on)
            {
                return;
            }

            _data.musicOn = on;
            Flush();
            MusicChanged?.Invoke(on);
        }

        public void SetSoundOn(bool on)
        {
            if (_data.soundOn == on)
            {
                return;
            }

            _data.soundOn = on;
            Flush();
            SoundChanged?.Invoke(on);
        }

        public void SetLanguage(string language)
        {
            if (_data.language == language)
            {
                return;
            }

            _data.language = language;
            Flush();
            LanguageChanged?.Invoke(language);
        }

        private static void Migrate(SaveData data)
        {
            // Версия 1 — первая. Сюда добавляются шаги переноса при изменении формата.
            if (data.version < SaveData.CurrentVersion)
            {
                data.version = SaveData.CurrentVersion;
            }

            if (data.language == null)
            {
                data.language = string.Empty;
            }
        }

        private void Flush()
        {
            _storage.Save(_data);
        }

        // Если в проекте отключат Domain Reload, статическое поле не переживёт перезапуск плеймода.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            _instance = null;
        }
    }
}
