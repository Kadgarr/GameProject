using UnityEngine;

namespace DarknessInside.Analytics
{
    /// <summary>
    /// Пишет события в консоль и никуда их не отправляет. Работает в редакторе и Development Build,
    /// чтобы проверять события без панели сервиса.
    /// </summary>
    public sealed class LogAnalyticsService : IAnalyticsService
    {
        private const string Prefix = "[Analytics] ";

        public void SetEnabled(bool enabled) => Log("sending " + (enabled ? "on" : "off"));

        public void LevelStarted(int level, int attempt) => Log("level_start level=" + level + " attempt=" + attempt);

        public void LevelCompleted(int level, int seconds, int deaths) =>
            Log("level_complete level=" + level + " duration_sec=" + seconds + " deaths=" + deaths);

        public void PlayerDied(int level, string cause, int seconds) =>
            Log("player_death level=" + level + " cause=" + cause + " sec=" + seconds);

        public void LevelQuit(int level, int seconds) => Log("level_quit level=" + level + " duration_sec=" + seconds);

        public void RiddleAttempted(int level, bool success) => Log("riddle_attempt level=" + level + " success=" + success);

        public void ItemCollected(int level, string itemId) => Log("item_collected level=" + level + " item=" + itemId);

        public void HintOpened(int level) => Log("hint_opened level=" + level);

        public void SettingChanged(string setting, string value) => Log("settings_changed " + setting + "=" + value);

        private static void Log(string message)
        {
            Debug.Log(Prefix + message);
        }
    }
}
