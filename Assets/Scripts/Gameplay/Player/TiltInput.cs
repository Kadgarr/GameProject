using UnityEngine;

namespace DarknessInside.Gameplay.Player
{
    /// <summary>
    /// Движение наклоном телефона (механика отдельных уровней). Прыжок — экранной кнопкой.
    /// Кнопки движения на таком уровне игнорируются. На компьютере акселерометра нет, поэтому там
    /// наклон заменяют клавиши оси Horizontal (A/D, стрелки).
    /// </summary>
    public sealed class TiltInput : PlayerInputBase
    {
        private const string KeyboardAxis = "Horizontal";

        [Tooltip("Во сколько раз усиливается наклон: при 1.8 полная скорость примерно при наклоне 33°.")]
        [SerializeField] private float sensitivity = 1.8f;
        [Tooltip("Наклон меньше этого значения считается нулём — игрок не «ползёт» на почти ровном телефоне.")]
        [SerializeField, Range(0f, 0.3f)] private float deadZone = 0.05f;

        private bool _useKeyboard;

        private void Awake()
        {
            _useKeyboard = !Application.isMobilePlatform;
        }

        public override float Horizontal
        {
            get
            {
                if (_useKeyboard)
                {
                    float keys = Input.GetAxisRaw(KeyboardAxis);
                    if (keys != 0f)
                    {
                        return Mathf.Clamp(keys, -1f, 1f);
                    }
                }

                float tilt = Input.acceleration.x;
                if (Mathf.Abs(tilt) < deadZone)
                {
                    return 0f;
                }

                return Mathf.Clamp(tilt * sensitivity, -1f, 1f);
            }
        }
    }
}
