using DarknessInside.Core;
using UnityEngine;

namespace DarknessInside.UI
{
    /// <summary>
    /// В браузере на телефоне ориентацию экрана не заблокировать, поэтому в портретной ориентации
    /// поверх игры показывается подсказка «поверните телефон». Пересчёт — по событию смены экрана.
    /// В приложении для Android ориентация зафиксирована, и подсказка не нужна.
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public sealed class OrientationHint : MonoBehaviour
    {
        [Tooltip("Показывать и вне браузера (для проверки в редакторе).")]
        [SerializeField] private bool showOutsideWeb;

        private Canvas _canvas;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }

        private void OnEnable()
        {
            ScreenWatcher.Changed += Refresh;
            Refresh();
        }

        private void OnDisable()
        {
            ScreenWatcher.Changed -= Refresh;
        }

        private void Refresh()
        {
            bool web = Application.platform == RuntimePlatform.WebGLPlayer || showOutsideWeb;
            _canvas.enabled = web && Screen.height > Screen.width;
        }
    }
}
