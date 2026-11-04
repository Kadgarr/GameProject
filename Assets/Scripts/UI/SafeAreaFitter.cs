using DarknessInside.Core;
using UnityEngine;

namespace DarknessInside.UI
{
    /// <summary>
    /// Растягивает панель на безопасную зону экрана (без выреза камеры и закруглённых углов).
    /// Кнопки внутри панели привязаны к её углам, поэтому не уходят под вырез.
    /// Пересчёт только при смене экрана (ScreenWatcher), не каждый кадр.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public sealed class SafeAreaFitter : MonoBehaviour
    {
        private RectTransform _rect;

        private void Awake()
        {
            _rect = (RectTransform)transform;
        }

        private void OnEnable()
        {
            Apply();
            ScreenWatcher.Changed += Apply;
        }

        private void OnDisable()
        {
            ScreenWatcher.Changed -= Apply;
        }

        private void Apply()
        {
            if (Screen.width <= 0 || Screen.height <= 0)
            {
                return;
            }

            Rect safe = Screen.safeArea;
            var min = new Vector2(safe.xMin / Screen.width, safe.yMin / Screen.height);
            var max = new Vector2(safe.xMax / Screen.width, safe.yMax / Screen.height);
            _rect.anchorMin = min;
            _rect.anchorMax = max;
            _rect.offsetMin = Vector2.zero;
            _rect.offsetMax = Vector2.zero;
        }
    }
}
