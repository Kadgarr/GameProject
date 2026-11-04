using System;
using UnityEngine;

namespace DarknessInside.Core
{
    /// <summary>
    /// Пауза игры: Time.timeScale и событие для тех, кому нужно среагировать
    /// (например, «отпустить» зажатые кнопки управления).
    /// </summary>
    public static class GamePause
    {
        public static bool IsPaused { get; private set; }

        public static event Action<bool> Changed;

        public static void Set(bool paused)
        {
            Time.timeScale = paused ? 0f : 1f;
            if (IsPaused == paused)
            {
                return;
            }

            IsPaused = paused;
            Changed?.Invoke(paused);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            IsPaused = false;
            Changed = null;
        }
    }
}
