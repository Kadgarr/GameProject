using DarknessInside.Core;
using DarknessInside.Gameplay.Cameras;
using UnityEngine;
using UnityEngine.UI;

namespace DarknessInside.UI
{
    /// <summary>
    /// Для канвасов, привязанных к миру (фон уровня, подписи загадок, кнопка у места постройки):
    /// когда CameraFit отдаляет камеру на узком экране, канвас уменьшается так же, чтобы картинка
    /// и надписи оставались на своих местах относительно уровня. Фоны с UICover при этом
    /// растягиваются так, чтобы закрыть весь экран.
    /// </summary>
    [RequireComponent(typeof(CanvasScaler))]
    public sealed class CanvasZoomFollower : MonoBehaviour
    {
        private CanvasScaler _scaler;
        private Vector2 _baseResolution;
        private UICover[] _covers;

        private void Awake()
        {
            _scaler = GetComponent<CanvasScaler>();
            _baseResolution = _scaler.referenceResolution;
            _covers = GetComponentsInChildren<UICover>(true);
        }

        private void OnEnable()
        {
            Apply(CameraFit.Zoom);
            CameraFit.ZoomChanged += Apply;
            ScreenWatcher.Changed += OnScreenChanged;
        }

        // Фоны (UICover) получают Awake позже канваса, поэтому применяем ещё раз, когда все готовы.
        private void Start()
        {
            Apply(CameraFit.Zoom);
        }

        private void OnDisable()
        {
            CameraFit.ZoomChanged -= Apply;
            ScreenWatcher.Changed -= OnScreenChanged;
        }

        private void OnScreenChanged()
        {
            Apply(CameraFit.Zoom);
        }

        private void Apply(float zoom)
        {
            _scaler.referenceResolution = _baseResolution * zoom;

            // Канвас масштабируется по высоте (match = 1), поэтому его размер в единицах известен сразу.
            float height = _scaler.referenceResolution.y;
            float aspect = Screen.height > 0 ? (float)Screen.width / Screen.height : 16f / 9f;
            var canvasSize = new Vector2(height * aspect, height);
            for (int i = 0; i < _covers.Length; i++)
            {
                _covers[i].Refresh(canvasSize);
            }
        }
    }
}
