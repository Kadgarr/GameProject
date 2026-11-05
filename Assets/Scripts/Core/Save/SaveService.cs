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

        /// <summary>Игрок разрешил (true) или запретил (false) аналитику.</summary>
        public event Action<bool> AnalyticsConsentChanged;

        public int LastCompletedLevel => _data.lastCompletedLevel;

        public bool HasProgress => _data.lastCompletedLevel > 0;

        public bool MusicOn => _data.musicOn;

        public bool SoundOn => _data.soundOn;

        public string Language => _data.language;

        public ConsentState AnalyticsConsent => (ConsentState)_data.analyticsConsent;

        /// <summary>
        /// ID игрока для аналитики: создаётся один раз и хранится в сохранении. Его же видит игрок в настройках,
        /// поэтому запрос на удаление данных можно найти в сервисе аналитики.
        /// </summary>
        public string GetOrCreateAnalyticsUserId()
        {
            if (string.IsNullOrEmpty(_data.analyticsUserId))
            {
                _data.analyticsUserId = Guid.NewGuid().ToString();
                Flush();
            }

            return _data.analyticsUserId;
        }

        /// <summary>
        /// Подменить сервис (тесты, удаление сохранения из меню редактора). null — заново прочитать файл.
        /// Подписчики старого сервиса переходят к новому, чтобы долгоживущие объекты (Core) не потеряли события.
        /// </summary>
        public static void SetInstance(SaveService service)
        {
            SaveService next = service ?? new SaveService(new JsonFileSaveStorage(JsonFileSaveStorage.DefaultPath));
            if (_instance != null && _instance != next)
            {
                next.ProgressChanged += _instance.ProgressChanged;
                next.MusicChanged += _instance.MusicChanged;
                next.SoundChanged += _instance.SoundChanged;
                next.LanguageChanged += _instance.LanguageChanged;
                next.AnalyticsConsentChanged += _instance.AnalyticsConsentChanged;
            }

            _instance = next;
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

        public void SetAnalyticsConsent(bool granted)
        {
            int value = (int)(granted ? ConsentState.Granted : ConsentState.Denied);
            if (_data.analyticsConsent == value)
            {
                return;
            }

            _data.analyticsConsent = value;
            Flush();
            AnalyticsConsentChanged?.Invoke(granted);
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

            if (data.analyticsUserId == null)
            {
                data.analyticsUserId = string.Empty;
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
