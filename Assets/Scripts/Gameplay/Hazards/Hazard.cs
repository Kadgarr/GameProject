using UnityEngine;

namespace DarknessInside.Gameplay.Hazards
{
    /// <summary>
    /// Общая логика ловушек: найти игрока по столкновению и убить его.
    /// Перезапуск уровня делает не ловушка, а LevelFlow по событию смерти игрока.
    /// </summary>
    public abstract class Hazard : MonoBehaviour, IHazard
    {
        protected const string PlayerTag = "Player";

        [Tooltip("Через сколько секунд после смерти перезапустится уровень.")]
        [SerializeField] protected float timer;

        public float RestartDelay => timer;

        /// <summary>Убить игрока, если target — игрок. Возвращает true, если убили.</summary>
        protected bool TryKill(GameObject target)
        {
            if (!target.CompareTag(PlayerTag))
            {
                return false;
            }

            DeadPlayer player = target.GetComponentInParent<DeadPlayer>();
            if (player == null)
            {
                Debug.LogWarning("Hazard: object tagged Player has no DeadPlayer.", target);
                return false;
            }

            player.Kill(timer);
            return true;
        }

        /// <summary>Убить конкретного игрока (используется таймерами вроде Scale).</summary>
        public void Kill(DeadPlayer player)
        {
            if (player != null)
            {
                player.Kill(timer);
            }
        }
    }
}
