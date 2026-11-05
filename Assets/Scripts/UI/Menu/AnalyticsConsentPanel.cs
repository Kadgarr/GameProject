using DarknessInside.Core.Save;
using UnityEngine;
using UnityEngine.UI;

namespace DarknessInside.UI.Menu
{
    /// <summary>
    /// Окно согласия на аналитику при первом запуске. Показывается, пока игрок не ответил;
    /// ответ хранится в сохранении и меняется в настройках (AnalyticsToggleButton).
    /// Скрывается выключением своего Canvas, как экраны меню.
    /// </summary>
    [RequireComponent(typeof(Canvas), typeof(CanvasGroup))]
    public sealed class AnalyticsConsentPanel : MonoBehaviour
    {
        private Canvas _canvas;
        private CanvasGroup _group;
        private GraphicRaycaster _raycaster;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _group = GetComponent<CanvasGroup>();
            _raycaster = GetComponent<GraphicRaycaster>();
        }

        private void Start()
        {
            SetVisible(SaveService.Instance.AnalyticsConsent == ConsentState.Unknown && DarknessInside.Analytics.AnalyticsReporter.IsAvailable);
        }

        /// <summary>Кнопка «Разрешить».</summary>
        public void Accept()
        {
            Answer(true);
        }

        /// <summary>Кнопка «Нет».</summary>
        public void Decline()
        {
            Answer(false);
        }

        private void Answer(bool granted)
        {
            SaveService.Instance.SetAnalyticsConsent(granted);
            SetVisible(false);
        }

        private void SetVisible(bool visible)
        {
            _canvas.enabled = visible;
            if (_raycaster != null)
            {
                _raycaster.enabled = visible;
            }

            _group.interactable = visible;
            _group.blocksRaycasts = visible;
        }
    }
}
