using UnityEngine;

namespace DarknessInside.Gameplay.Collectibles
{
    /// <summary>Предмет на уровне: при касании игроком засчитывается в цель уровня и пропадает.</summary>
    [RequireComponent(typeof(Collider2D))]
    public sealed class CollectiblePickup : MonoBehaviour
    {
        private const string PlayerTag = "Player";

        [SerializeField] private CollectibleDefinition item;
        [SerializeField] private LevelCollectionGoal goal;

        public CollectibleDefinition Item => item;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(PlayerTag))
            {
                return;
            }

            if (goal != null)
            {
                goal.Collect(item);
            }

            gameObject.SetActive(false);
        }
    }
}
