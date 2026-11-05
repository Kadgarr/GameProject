using System;

namespace DarknessInside.Core.Save
{
    /// <summary>
    /// Всё, что игра хранит между запусками. Один JSON-файл; при изменении формата
    /// увеличиваем <see cref="CurrentVersion"/> и дописываем миграцию в <see cref="SaveService"/>.
    /// </summary>
    [Serializable]
    public sealed class SaveData
    {
        public const int CurrentVersion = 1;

        public int version = CurrentVersion;

        /// <summary>Номер последнего пройденного уровня (1..N), 0 — ничего не пройдено.</summary>
        public int lastCompletedLevel;

        public bool musicOn = true;
        public bool soundOn = true;

        /// <summary>Код языка (ru_RU, ua_UA, en_US). Пусто — выбрать по языку системы.</summary>
        public string language = string.Empty;

        /// <summary>Согласие на аналитику: 0 — не спрашивали, 1 — разрешено, 2 — запрещено (<see cref="ConsentState"/>).</summary>
        public int analyticsConsent;

        /// <summary>Случайный ID игрока для аналитики. Показывается в настройках, чтобы по нему можно было запросить удаление данных.</summary>
        public string analyticsUserId = string.Empty;
    }

    public enum ConsentState
    {
        Unknown = 0,
        Granted = 1,
        Denied = 2,
    }
}
