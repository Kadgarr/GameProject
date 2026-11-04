using DarknessInside.Core.Localization;
using DarknessInside.Core.Save;
using TMPro;
using UnityEngine;

namespace DarknessInside.UI.Menu
{
    /// <summary>
    /// Кнопка в настройках «Статистика: вкл/выкл» — отзыв или выдача согласия на аналитику.
    /// Подпись обновляется по событиям (смена языка и согласия), без опроса.
    /// </summary>
    public sealed class AnalyticsToggleButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;
        [SerializeField] private string onKey = "analytics_on";
        [SerializeField] private string offKey = "analytics_off";

        private void OnEnable()
        {
            LocalizationService.LanguageChanged += Refresh;
            SaveService.Instance.AnalyticsConsentChanged += OnConsentChanged;
            Refresh();
        }

        private void OnDisable()
        {
            LocalizationService.LanguageChanged -= Refresh;
            SaveService.Instance.AnalyticsConsentChanged -= OnConsentChanged;
        }

        /// <summary>Вызывается кнопкой.</summary>
        public void Toggle()
        {
            SaveService.Instance.SetAnalyticsConsent(!IsGranted);
        }

        private static bool IsGranted => SaveService.Instance.AnalyticsConsent == ConsentState.Granted;

        private void OnConsentChanged(bool granted)
        {
            Refresh();
        }

        private void Refresh()
        {
            LocalizationService localization = LocalizationService.Instance;
            if (label != null && localization != null)
            {
                label.text = localization.Get(IsGranted ? onKey : offKey);
            }
        }
    }
}
