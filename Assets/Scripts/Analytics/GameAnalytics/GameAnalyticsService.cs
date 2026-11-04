using System.Text;
using GameAnalyticsSDK;
using GA = GameAnalyticsSDK.GameAnalytics;

namespace DarknessInside.Analytics.GameAnalyticsAdapter
{
    /// <summary>
    /// Адаптер IAnalyticsService → GameAnalytics. Уровни — progression-события (воронка Start → Fail/Complete
    /// строится в панели сама), остальное — design-события вида "death:level_06:thorns".
    /// </summary>
    public sealed class GameAnalyticsService : IAnalyticsService
    {
        private const int MaxLevels = 100;
        private const int MaxPartLength = 64;

        private static readonly string[] LevelIds = BuildLevelIds();

        public void SetEnabled(bool enabled)
        {
            GA.SetEnabledEventSubmission(enabled);
        }

        public void LevelStarted(int level, int attempt)
        {
            GA.NewProgressionEvent(GAProgressionStatus.Start, LevelId(level));
        }

        public void LevelCompleted(int level, int seconds, int deaths)
        {
            string id = LevelId(level);
            GA.NewProgressionEvent(GAProgressionStatus.Complete, id, deaths);
            GA.NewDesignEvent("level_time:" + id, seconds);
        }

        public void PlayerDied(int level, string cause, int seconds)
        {
            string id = LevelId(level);
            GA.NewProgressionEvent(GAProgressionStatus.Fail, id);
            GA.NewDesignEvent("death:" + id + ":" + Sanitize(cause), seconds);
        }

        public void LevelQuit(int level, int seconds)
        {
            GA.NewDesignEvent("quit:" + LevelId(level), seconds);
        }

        public void RiddleAttempted(int level, bool success)
        {
            GA.NewDesignEvent("riddle:" + LevelId(level) + (success ? ":solved" : ":wrong"));
        }

        public void ItemCollected(int level, string itemId)
        {
            GA.NewDesignEvent("item:" + LevelId(level) + ":" + Sanitize(itemId));
        }

        public void HintOpened(int level)
        {
            GA.NewDesignEvent("hint:" + LevelId(level));
        }

        public void SettingChanged(string setting, string value)
        {
            GA.NewDesignEvent("settings:" + Sanitize(setting) + ":" + Sanitize(value));
        }

        private static string LevelId(int level)
        {
            return level > 0 && level < MaxLevels ? LevelIds[level] : "level_" + level;
        }

        private static string[] BuildLevelIds()
        {
            var ids = new string[MaxLevels];
            for (int i = 0; i < MaxLevels; i++)
            {
                ids[i] = "level_" + i.ToString("00");
            }

            return ids;
        }

        /// <summary>GameAnalytics принимает в частях события только латиницу, цифры и -_.()!? до 64 символов.</summary>
        private static string Sanitize(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "unknown";
            }

            var builder = new StringBuilder(value.Length);
            for (int i = 0; i < value.Length && builder.Length < MaxPartLength; i++)
            {
                char c = value[i];
                bool allowed = (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') ||
                               c == '-' || c == '_' || c == '.' || c == '(' || c == ')' || c == '!' || c == '?';
                builder.Append(allowed ? c : '_');
            }

            return builder.ToString();
        }
    }
}
