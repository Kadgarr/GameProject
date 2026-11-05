using UnityEngine;

namespace DarknessInside.Analytics
{
    /// <summary>
    /// Объект, который имеет смысл только при работающей аналитике (переключатель «Статистика», «Ваш ID»).
    /// На платформах, где сервис не настроен (например, WebGL), объект выключается.
    /// </summary>
    public sealed class AnalyticsOnlyObject : MonoBehaviour
    {
        private void Awake()
        {
            if (!AnalyticsReporter.IsAvailable)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
