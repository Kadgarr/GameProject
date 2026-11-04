using UnityEngine;
using UnityEngine.UI;

namespace DarknessInside.UI.Menu
{
    /// <summary>
    /// Экран меню — вложенный Canvas внутри общего MenuCanvas. Скрывается выключением компонента Canvas,
    /// а не GameObject: соседние экраны не перестраиваются, OnEnable/OnDisable детей не дёргаются.
    /// CanvasGroup гарантирует, что скрытый экран не ловит нажатия.
    /// </summary>
    [RequireComponent(typeof(Canvas), typeof(CanvasGroup))]
    public sealed class MenuScreen : MonoBehaviour
    {
        [SerializeField] private MenuScreenId id;

        private Canvas _canvas;
        private CanvasGroup _group;
        private GraphicRaycaster _raycaster;

        public MenuScreenId Id => id;

        public bool IsVisible => Cache() && _canvas.enabled;

        public void SetVisible(bool visible)
        {
            if (!Cache())
            {
                return;
            }

            _canvas.enabled = visible;
            if (_raycaster != null)
            {
                _raycaster.enabled = visible;
            }

            _group.interactable = visible;
            _group.blocksRaycasts = visible;
        }

        private bool Cache()
        {
            if (_canvas == null)
            {
                _canvas = GetComponent<Canvas>();
                _group = GetComponent<CanvasGroup>();
                _raycaster = GetComponent<GraphicRaycaster>();
            }

            return _canvas != null;
        }
    }
}
