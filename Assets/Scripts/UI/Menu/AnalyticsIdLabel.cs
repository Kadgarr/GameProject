using System.Collections;
using DarknessInside.Core.Localization;
using DarknessInside.Core.Save;
using TMPro;
using UnityEngine;

namespace DarknessInside.UI.Menu
{
    /// <summary>
    /// Показывает ID игрока для аналитики («Ваш ID: …») в настройках. По нажатию копирует ID в буфер обмена,
    /// чтобы его было удобно отправить вместе с запросом на удаление данных.
    /// </summary>
    public sealed class AnalyticsIdLabel : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;
        [Tooltip("Ключ локализации с {0} на месте ID.")]
        [SerializeField] private string formatKey = "analytics_id";
        [SerializeField] private string copiedKey = "analytics_id_copied";
        [SerializeField] private float copiedMessageSeconds = 1.5f;

        private WaitForSecondsRealtime _copiedDelay;
        private Coroutine _copiedRoutine;

        private void Awake()
        {
            _copiedDelay = new WaitForSecondsRealtime(copiedMessageSeconds);
        }

        private void OnEnable()
        {
            LocalizationService.LanguageChanged += Refresh;
            Refresh();
        }

        private void OnDisable()
        {
            LocalizationService.LanguageChanged -= Refresh;
            _copiedRoutine = null;
        }

        /// <summary>Вызывается кнопкой: скопировать ID.</summary>
        public void Copy()
        {
            GUIUtility.systemCopyBuffer = SaveService.Instance.GetOrCreateAnalyticsUserId();
            if (_copiedRoutine != null)
            {
                StopCoroutine(_copiedRoutine);
            }

            _copiedRoutine = StartCoroutine(ShowCopied());
        }

        private IEnumerator ShowCopied()
        {
            SetText(Localize(copiedKey));
            yield return _copiedDelay;
            _copiedRoutine = null;
            Refresh();
        }

        private void Refresh()
        {
            if (_copiedRoutine != null)
            {
                return;
            }

            string format = Localize(formatKey);
            string id = SaveService.Instance.GetOrCreateAnalyticsUserId();
            SetText(format.Contains("{0}") ? string.Format(format, id) : format + " " + id);
        }

        private static string Localize(string key)
        {
            LocalizationService localization = LocalizationService.Instance;
            return localization != null ? localization.Get(key) : key;
        }

        private void SetText(string text)
        {
            if (label != null)
            {
                label.text = text;
            }
        }
    }
}
