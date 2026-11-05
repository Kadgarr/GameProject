using GameAnalyticsSDK.Setup;
using UnityEngine;
using GA = GameAnalyticsSDK.GameAnalytics;

namespace DarknessInside.Analytics.GameAnalyticsAdapter
{
    /// <summary>
    /// Создаёт объект GameAnalytics из префаба и инициализирует SDK. Ключи игры задаются в
    /// Window → GameAnalytics → Select Settings (Resources/GameAnalytics/Settings), не в коде.
    /// </summary>
    [CreateAssetMenu(fileName = "GameAnalytics", menuName = "Darkness Inside/Analytics/GameAnalytics")]
    public sealed class GameAnalyticsServiceFactory : AnalyticsServiceFactory
    {
        private const string SettingsPath = "GameAnalytics/Settings";

        [Tooltip("Префаб с компонентом GameAnalytics (SDK требует такой объект в сцене).")]
        [SerializeField] private GameObject gameAnalyticsPrefab;

        public override IAnalyticsService Create(string userId)
        {
            if (!IsConfigured())
            {
                Debug.LogWarning("GameAnalytics: нет ключей для платформы " + Application.platform +
                                 ". Window → GameAnalytics → Select Settings.");
                return null;
            }

            if (gameAnalyticsPrefab != null && Object.FindFirstObjectByType<GA>() == null)
            {
                Object.Instantiate(gameAnalyticsPrefab).name = gameAnalyticsPrefab.name;
            }

            if (!string.IsNullOrEmpty(userId))
            {
                // Свой ID вместо случайного ID SDK: игрок видит его в настройках и может прислать для удаления данных.
                GA.SetCustomId(userId);
            }

            GA.Initialize();
            return new GameAnalyticsService();
        }

        /// <summary>В редакторе считаем доступным, чтобы проверять окно согласия; в сборке — только при ключах для платформы.</summary>
        public override bool IsAvailable => Application.isEditor || IsConfigured();

        private static bool IsConfigured()
        {
            var settings = Resources.Load<Settings>(SettingsPath);
            if (settings == null)
            {
                return false;
            }

            int index = settings.Platforms.IndexOf(Application.platform);
            return index >= 0 &&
                   !string.IsNullOrEmpty(settings.GetGameKey(index)) &&
                   !string.IsNullOrEmpty(settings.GetSecretKey(index));
        }
    }
}
