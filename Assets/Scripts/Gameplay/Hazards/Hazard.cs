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

        [Tooltip("Причина смерти для аналитики. Пусто — по типу ловушки.")]
        [SerializeField] private string causeId;

        public float RestartDelay => timer;

        /// <summary>Причина смерти для аналитики.</summary>
        public string Cause => string.IsNullOrEmpty(causeId) ? DefaultCause : causeId;

        /// <summary>Причина по умолчанию для этого типа ловушки.</summary>
        protected abstract string DefaultCause { get; }

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

            player.Kill(timer, Cause);
            return true;
        }

        /// <summary>Убить конкретного игрока (используется таймерами вроде Scale).</summary>
        public void Kill(DeadPlayer player)
        {
            if (player != null)
            {
                player.Kill(timer, Cause);
            }
        }
    }
}
