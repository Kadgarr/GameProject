using UnityEngine;

namespace DarknessInside.Analytics
{
    /// <summary>
    /// Ассет, который создаёт сервис аналитики (Abstract Factory). AnalyticsReporter держит список таких
    /// ассетов: чтобы сменить или добавить сервис, достаточно поменять ассет в инспекторе.
    /// </summary>
    public abstract class AnalyticsServiceFactory : ScriptableObject
    {
        /// <summary>
        /// Создать и инициализировать сервис. userId — ID игрока из сохранения (тот же, что показан в настройках).
        /// null — сервис не настроен для этой платформы.
        /// </summary>
        public abstract IAnalyticsService Create(string userId);

        /// <summary>Настроен ли сервис для текущей платформы (есть ключи и т. п.). От этого зависит, спрашивать ли согласие.</summary>
        public virtual bool IsAvailable => true;
    }
}
