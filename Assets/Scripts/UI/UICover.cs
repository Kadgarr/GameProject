using UnityEngine;

namespace DarknessInside.UI
{
    /// <summary>
    /// Фоновая картинка на канвасе, привязанном к миру: увеличивается ровно настолько, чтобы закрыть
    /// весь экран (без синих полос на 16:9 и 21:9), и никогда не уменьшается — на экранах, под которые
    /// уровень рисовался, фон остаётся как был. Пересчитывает CanvasZoomFollower при смене экрана или зума.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public sealed class UICover : MonoBehaviour
    {
        private RectTransform _rect;
        private Vector2 _size;

        private void Awake()
        {
            _rect = (RectTransform)transform;
            _size = _rect.rect.size;
        }

        /// <param name="canvasSize">Размер канваса в его единицах.</param>
        public void Refresh(Vector2 canvasSize)
        {
            if (_rect == null || _size.x <= 0f || _size.y <= 0f)
            {
                return;
            }

            // Центр картинки может быть немного смещён — учитываем, чтобы край не открылся.
            Vector2 offset = _rect.anchoredPosition;
            float needX = (canvasSize.x * 0.5f + Mathf.Abs(offset.x)) * 2f / _size.x;
            float needY = (canvasSize.y * 0.5f + Mathf.Abs(offset.y)) * 2f / _size.y;
            float scale = Mathf.Max(1f, Mathf.Max(needX, needY));
            _rect.localScale = new Vector3(scale, scale, 1f);
        }
    }
}
