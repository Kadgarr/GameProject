using System.Collections.Generic;

namespace DarknessInside.Analytics
{
    /// <summary>Рассылает каждое событие во все подключённые сервисы (Composite).</summary>
    public sealed class CompositeAnalyticsService : IAnalyticsService
    {
        private readonly List<IAnalyticsService> _services = new List<IAnalyticsService>();

        public int Count => _services.Count;

        public void Add(IAnalyticsService service)
        {
            if (service != null && !_services.Contains(service))
            {
                _services.Add(service);
            }
        }

        public void Remove(IAnalyticsService service)
        {
            _services.Remove(service);
        }

        public void SetEnabled(bool enabled)
        {
            for (int i = 0; i < _services.Count; i++)
            {
                _services[i].SetEnabled(enabled);
            }
        }

        public void LevelStarted(int level, int attempt)
        {
            for (int i = 0; i < _services.Count; i++)
            {
                _services[i].LevelStarted(level, attempt);
            }
        }

        public void LevelCompleted(int level, int seconds, int deaths)
        {
            for (int i = 0; i < _services.Count; i++)
            {
                _services[i].LevelCompleted(level, seconds, deaths);
            }
        }

        public void PlayerDied(int level, string cause, int seconds)
        {
            for (int i = 0; i < _services.Count; i++)
            {
                _services[i].PlayerDied(level, cause, seconds);
            }
        }

        public void LevelQuit(int level, int seconds)
        {
            for (int i = 0; i < _services.Count; i++)
            {
                _services[i].LevelQuit(level, seconds);
            }
        }

        public void RiddleAttempted(int level, bool success)
        {
            for (int i = 0; i < _services.Count; i++)
            {
                _services[i].RiddleAttempted(level, success);
            }
        }

        public void ItemCollected(int level, string itemId)
        {
            for (int i = 0; i < _services.Count; i++)
            {
                _services[i].ItemCollected(level, itemId);
            }
        }

        public void HintOpened(int level)
        {
            for (int i = 0; i < _services.Count; i++)
            {
                _services[i].HintOpened(level);
            }
        }

        public void SettingChanged(string setting, string value)
        {
            for (int i = 0; i < _services.Count; i++)
            {
                _services[i].SettingChanged(setting, value);
            }
        }
    }
}
