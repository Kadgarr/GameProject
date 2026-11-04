namespace DarknessInside.Analytics
{
    /// <summary>
    /// События игры на языке игры (а не конкретного сервиса). Каждый сервис аналитики — адаптер этого
    /// интерфейса, поэтому сервис меняется без правок игрового кода.
    /// </summary>
    public interface IAnalyticsService
    {
        /// <summary>Разрешить или запретить отправку (согласие игрока).</summary>
        void SetEnabled(bool enabled);

        /// <summary>Уровень загружен. attempt — номер попытки подряд (1 — первая).</summary>
        void LevelStarted(int level, int attempt);

        /// <summary>Уровень пройден за seconds (с первой попытки подряд) и deaths смертей.</summary>
        void LevelCompleted(int level, int seconds, int deaths);

        /// <summary>Игрок погиб от cause через seconds после начала попытки.</summary>
        void PlayerDied(int level, string cause, int seconds);

        /// <summary>Игрок вышел в меню с уровня, проведя на нём seconds.</summary>
        void LevelQuit(int level, int seconds);

        void RiddleAttempted(int level, bool success);

        void ItemCollected(int level, string itemId);

        void HintOpened(int level);

        void SettingChanged(string setting, string value);
    }
}
