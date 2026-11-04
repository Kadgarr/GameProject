using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarknessInside.Gameplay.Collectibles
{
    /// <summary>
    /// Список предметов, которые нужно собрать на уровне, и их текущее состояние.
    /// Остальные объекты узнают об изменениях через события (без опроса в Update).
    /// </summary>
    public sealed class LevelCollectionGoal : MonoBehaviour
    {
        [SerializeField] private List<CollectibleDefinition> requiredItems = new List<CollectibleDefinition>();

        [Header("Звук подбора")]
        [Tooltip("Источник звуковых эффектов уровня (учитывает настройку «Звук вкл/выкл»).")]
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioClip collectClip;

        private readonly HashSet<CollectibleDefinition> _collected = new HashSet<CollectibleDefinition>();

        /// <summary>Подобран предмет.</summary>
        public event Action<CollectibleDefinition> ItemCollected;

        /// <summary>Игрок пытался что-то построить без нужных предметов — HUD подсвечивает недостающие.</summary>
        public event Action<IReadOnlyList<CollectibleDefinition>> MissingItemsRequested;

        /// <summary>Собраны все предметы уровня.</summary>
        public event Action Completed;

        public IReadOnlyList<CollectibleDefinition> RequiredItems => requiredItems;

        public bool IsCollected(CollectibleDefinition item) => item != null && _collected.Contains(item);

        public bool HasAll(IReadOnlyList<CollectibleDefinition> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (!IsCollected(items[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public void Collect(CollectibleDefinition item)
        {
            if (item == null || !_collected.Add(item))
            {
                return;
            }

            if (sfxSource != null && sfxSource.isActiveAndEnabled && collectClip != null)
            {
                sfxSource.PlayOneShot(collectClip);
            }

            ItemCollected?.Invoke(item);

            if (HasAll(requiredItems))
            {
                Completed?.Invoke();
            }
        }

        public void ReportMissing(IReadOnlyList<CollectibleDefinition> items) => MissingItemsRequested?.Invoke(items);
    }
}
