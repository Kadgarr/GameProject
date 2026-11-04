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

        public override IAnalyticsService Create()
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

            GA.Initialize();
            return new GameAnalyticsService();
        }

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
