using UnityEngine;

namespace DarknessInside.Core
{
    /// <summary>
    /// Частота кадров игры. Без явного значения Android ограничивает игру 30 FPS.
    /// Стоит в префабе Core, значение правится в инспекторе.
    /// </summary>
    public sealed class FrameRateSettings : MonoBehaviour
    {
        [Tooltip("Целевая частота кадров. 60 — плавно на большинстве телефонов; 30 — экономия батареи.")]
        [SerializeField, Range(30, 120)] private int targetFrameRate = 60;

        private void Awake()
        {
            Application.targetFrameRate = targetFrameRate;
        }
    }
}
