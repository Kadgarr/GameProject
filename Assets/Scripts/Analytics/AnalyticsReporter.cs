using DarknessInside.Core.Save;
using DarknessInside.Gameplay.Levels;
using UnityEngine;

namespace DarknessInside.Analytics
{
    /// <summary>
    /// Слушает события игры (LevelFlow, настройки) и пересылает их в сервисы аналитики. Живёт в Core,
    /// поэтому видит все уровни подряд: считает попытки, смерти и время прохождения.
    /// Внешние сервисы создаются только после согласия игрока; лог в консоль — локальный и работает всегда
    /// (в редакторе и Development Build).
    /// </summary>
    public sealed class AnalyticsReporter : MonoBehaviour
    {
        private const string SettingMusic = "music";
        private const string SettingSound = "sound";
        private const string SettingLanguage = "language";
        private const string On = "on";
        private const string Off = "off";

        [Tooltip("Сервисы, куда отправляются события (ассеты-фабрики). Создаются после согласия игрока.")]
        [SerializeField] private AnalyticsServiceFactory[] providers = new AnalyticsServiceFactory[0];

        [Tooltip("Отправлять события из редактора. Обычно выключено, чтобы не засорять статистику.")]
        [SerializeField] private bool sendFromEditor;

        [Tooltip("Писать события в консоль (только редактор и Development Build).")]
        [SerializeField] private bool logToConsole = true;

        private readonly CompositeAnalyticsService _local = new CompositeAnalyticsService();
        private readonly CompositeAnalyticsService _remote = new CompositeAnalyticsService();
        private readonly CompositeAnalyticsService _all = new CompositeAnalyticsService();

        private bool _remoteCreated;
        private int _level;
        private int _attempt;
        private int _deaths;
        private float _runStart;
        private float _attemptStart;

        public static AnalyticsReporter Instance { get; private set; }

        private bool CanSendRemote => !Application.isEditor || sendFromEditor;

        private void Awake()
        {
            Instance = this;
            if (logToConsole && (Application.isEditor || Debug.isDebugBuild))
            {
                _local.Add(new LogAnalyticsService());
            }

            _all.Add(_local);
            _all.Add(_remote);
        }

        private void OnEnable()
        {
            LevelFlow.LevelStarted += OnLevelStarted;
            LevelFlow.PlayerDied += OnPlayerDied;
            LevelFlow.LevelCompleted += OnLevelCompleted;
            LevelFlow.LevelQuit += OnLevelQuit;
            LevelFlow.RiddleAttempted += OnRiddleAttempted;
            LevelFlow.ItemCollected += OnItemCollected;
            LevelFlow.HintOpened += OnHintOpened;

            SaveService save = SaveService.Instance;
            save.MusicChanged += OnMusicChanged;
            save.SoundChanged += OnSoundChanged;
            save.LanguageChanged += OnLanguageChanged;
            save.AnalyticsConsentChanged += ApplyConsent;
            ApplyConsent(save.AnalyticsConsent == ConsentState.Granted);
        }

        private void OnDisable()
        {
            LevelFlow.LevelStarted -= OnLevelStarted;
            LevelFlow.PlayerDied -= OnPlayerDied;
            LevelFlow.LevelCompleted -= OnLevelCompleted;
            LevelFlow.LevelQuit -= OnLevelQuit;
            LevelFlow.RiddleAttempted -= OnRiddleAttempted;
            LevelFlow.ItemCollected -= OnItemCollected;
            LevelFlow.HintOpened -= OnHintOpened;

            SaveService save = SaveService.Instance;
            save.MusicChanged -= OnMusicChanged;
            save.SoundChanged -= OnSoundChanged;
            save.LanguageChanged -= OnLanguageChanged;
            save.AnalyticsConsentChanged -= ApplyConsent;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        /// <summary>Подключить локальный приёмник событий (тесты, отладочные панели). Согласие на него не влияет.</summary>
        public void Register(IAnalyticsService service)
        {
            _local.Add(service);
        }

        public void Unregister(IAnalyticsService service)
        {
            _local.Remove(service);
        }

        private void ApplyConsent(bool granted)
        {
            if (granted && !_remoteCreated && CanSendRemote)
            {
                _remoteCreated = true;
                string userId = SaveService.Instance.GetOrCreateAnalyticsUserId();
                for (int i = 0; i < providers.Length; i++)
                {
                    IAnalyticsService service = providers[i] != null ? providers[i].Create(userId) : null;
                    _remote.Add(service);
                }
            }

            _remote.SetEnabled(granted);
        }

        private void OnLevelStarted(int level)
        {
            if (level <= 0)
            {
                return;
            }

            float now = Time.time;
            if (level != _level)
            {
                _level = level;
                _attempt = 0;
                _deaths = 0;
                _runStart = now;
            }

            _attempt++;
            _attemptStart = now;
            _all.LevelStarted(level, _attempt);
        }

        private void OnPlayerDied(int level, string cause)
        {
            if (level <= 0)
            {
                return;
            }

            _deaths++;
            _all.PlayerDied(level, cause, Seconds(_attemptStart));
        }

        private void OnLevelCompleted(int level)
        {
            if (level <= 0)
            {
                return;
            }

            _all.LevelCompleted(level, Seconds(_runStart), _deaths);
            _level = 0;
        }

        private void OnLevelQuit(int level)
        {
            if (level <= 0)
            {
                return;
            }

            _all.LevelQuit(level, Seconds(_runStart));
            _level = 0;
        }

        private void OnRiddleAttempted(int level, bool success) => _all.RiddleAttempted(level, success);

        private void OnItemCollected(int level, string itemId) => _all.ItemCollected(level, itemId);

        private void OnHintOpened(int level) => _all.HintOpened(level);

        private void OnMusicChanged(bool on) => _all.SettingChanged(SettingMusic, on ? On : Off);

        private void OnSoundChanged(bool on) => _all.SettingChanged(SettingSound, on ? On : Off);

        private void OnLanguageChanged(string language) => _all.SettingChanged(SettingLanguage, language);

        private static int Seconds(float since) => Mathf.Max(0, Mathf.RoundToInt(Time.time - since));
    }
}
