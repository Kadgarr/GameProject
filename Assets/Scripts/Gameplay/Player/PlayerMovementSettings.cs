using UnityEngine;

namespace DarknessInside.Gameplay.Player
{
    /// <summary>Параметры движения игрока. Подбираются на телефоне без правки кода.</summary>
    [CreateAssetMenu(menuName = "Darkness Inside/Player Movement Settings", fileName = "PlayerMovement")]
    public sealed class PlayerMovementSettings : ScriptableObject
    {
        [Tooltip("Скорость бега, единиц в секунду.")]
        [Min(0f)] public float moveSpeed = 3.8f;

        [Tooltip("Импульс прыжка.")]
        [Min(0f)] public float jumpImpulse = 12.8f;
    }
}
