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
    }
}
