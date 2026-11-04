using System;
using DarknessInside.Core;
using UnityEngine;

namespace DarknessInside.Gameplay.Cameras
{
    /// <summary>
    /// Подгоняет камеру под экран: на широких телефонах (20:9, 21:9) высота кадра — как задумано,
    /// а на узких (16:9) камера отдаляется ровно настолько, чтобы уровень от стены до стены
    /// помещался по ширине. Канвасы, привязанные к миру, следуют за этим через событие ZoomChanged.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public sealed class CameraFit : MonoBehaviour
    {
        [Tooltip("Высота кадра по замыслу (Orthographic Size). На широких экранах камера остаётся такой.")]
        [SerializeField] private float designOrthographicSize = 4.731633f;

        [Tooltip("Сколько единиц по горизонтали от центра камеры должно быть видно всегда (до стен уровня).")]
        [SerializeField] private float requiredHalfWidth = 9f;

        private Camera _camera;

        /// <summary>Во сколько раз камера отдалена относительно замысла (1 — не отдалена).</summary>
        public static float Zoom { get; private set; } = 1f;

        public static event Action<float> ZoomChanged;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            Apply();
        }

        private void OnEnable()
        {
            ScreenWatcher.Changed += Apply;
        }

        private void OnDisable()
        {
            ScreenWatcher.Changed -= Apply;
        }

        private void Apply()
        {
            float aspect = _camera.aspect;
            float size = Mathf.Max(designOrthographicSize, requiredHalfWidth / Mathf.Max(aspect, 0.01f));
            _camera.orthographicSize = size;

            float zoom = size / designOrthographicSize;
            if (!Mathf.Approximately(zoom, Zoom))
            {
                Zoom = zoom;
                ZoomChanged?.Invoke(zoom);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Vector3 center = transform.position;
            center.z = 0f;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(center, new Vector3(requiredHalfWidth * 2f, designOrthographicSize * 2f, 0f));
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            Zoom = 1f;
            ZoomChanged = null;
        }
    }
}
