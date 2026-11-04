using UnityEngine;

namespace DarknessInside.Gameplay.Player
{
    /// <summary>Параметры движения игрока. Подбираются на телефоне без правки кода (можно менять в плеймоде).</summary>
    [CreateAssetMenu(menuName = "Darkness Inside/Player Movement Settings", fileName = "PlayerMovement")]
    public sealed class PlayerMovementSettings : ScriptableObject
    {
        [Header("Бег")]
        [Tooltip("Скорость бега, единиц в секунду.")]
        [Min(0f)] public float moveSpeed = 3.8f;

        [Tooltip("Разгон на земле, единиц/с². Чем больше, тем резче старт (60 — полная скорость за ~0.06 с).")]
        [Min(0f)] public float groundAcceleration = 60f;

        [Tooltip("Торможение на земле, единиц/с².")]
        [Min(0f)] public float groundDeceleration = 80f;

        [Tooltip("Разгон и торможение в воздухе, единиц/с². Меньше, чем на земле, — прыжок ощущается весомее.")]
        [Min(0f)] public float airAcceleration = 35f;

        [Header("Прыжок")]
        [Tooltip("Импульс прыжка.")]
        [Min(0f)] public float jumpImpulse = 12.8f;

        [Tooltip("Сколько секунд после схода с края ещё можно прыгнуть (coyote time).")]
        [Range(0f, 0.3f)] public float coyoteTime = 0.1f;

        [Tooltip("Сколько секунд помнится нажатие прыжка до приземления (буфер прыжка).")]
        [Range(0f, 0.3f)] public float jumpBufferTime = 0.1f;

        [Header("Проверка земли")]
        [Tooltip("Слои, на которых можно стоять и от которых можно прыгать: Ground, Platform и UI (кнопки-площадки на уровнях 3 и 11).")]
        public LayerMask groundLayers = (1 << 5) | (1 << 9) | (1 << 11);

        [Tooltip("Высота зоны проверки под ногами, единиц.")]
        [Range(0.01f, 0.3f)] public float groundCheckDepth = 0.06f;

        [Tooltip("Ширина зоны проверки относительно ширины коллайдера игрока.")]
        [Range(0.3f, 1f)] public float groundCheckWidth = 0.8f;
    }
}
