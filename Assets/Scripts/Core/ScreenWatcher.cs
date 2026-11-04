using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DarknessInside.Core
{
    /// <summary>
    /// Сообщает об изменении размера экрана или безопасной зоны (поворот, разделённый экран, складной телефон).
    /// Висит на пустом канвасе в Core: Unity сама вызывает OnRectTransformDimensionsChange при смене размера,
    /// поэтому опроса в Update нет.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public sealed class ScreenWatcher : UIBehaviour
    {
        private static int _width;
        private static int _height;
        private static Rect _safeArea;

        public static event Action Changed;

        protected override void OnEnable()
        {
            base.OnEnable();
            Check();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            Check();
        }

        private static void Check()
        {
            if (Screen.width == _width && Screen.height == _height && Screen.safeArea == _safeArea)
            {
                return;
            }

            _width = Screen.width;
            _height = Screen.height;
            _safeArea = Screen.safeArea;
            Changed?.Invoke();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            Changed = null;
            _width = 0;
            _height = 0;
            _safeArea = default;
        }
    }
}
