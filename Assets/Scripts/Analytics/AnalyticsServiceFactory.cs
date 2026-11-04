using UnityEngine;

namespace DarknessInside.Analytics
{
    /// <summary>
    /// Ассет, который создаёт сервис аналитики (Abstract Factory). AnalyticsReporter держит список таких
    /// ассетов: чтобы сменить или добавить сервис, достаточно поменять ассет в инспекторе.
    /// </summary>
    public abstract class AnalyticsServiceFactory : ScriptableObject
    {
        /// <summary>Создать и инициализировать сервис. null — сервис не настроен для этой платформы.</summary>
        public abstract IAnalyticsService Create();
    }
}
