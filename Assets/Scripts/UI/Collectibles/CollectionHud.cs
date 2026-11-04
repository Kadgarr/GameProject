using System.Collections.Generic;
using DarknessInside.Gameplay.Collectibles;
using UnityEngine;

namespace DarknessInside.UI.Collectibles
{
    /// <summary>
    /// Полоска вверху экрана: что нужно собрать на уровне. Слоты расставлены в сцене/префабе
    /// и правятся в редакторе; HUD только реагирует на события цели уровня.
    /// </summary>
    public sealed class CollectionHud : MonoBehaviour
    {
        [SerializeField] private LevelCollectionGoal goal;
        [SerializeField] private List<CollectionSlot> slots = new List<CollectionSlot>();

        private void OnEnable()
        {
            if (goal == null)
            {
                return;
            }

            goal.ItemCollected += OnItemCollected;
            goal.MissingItemsRequested += OnMissingItemsRequested;

            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].SetCollected(goal.IsCollected(slots[i].Item), false);
            }
        }

        private void OnDisable()
        {
            if (goal == null)
            {
                return;
            }

            goal.ItemCollected -= OnItemCollected;
            goal.MissingItemsRequested -= OnMissingItemsRequested;
        }

        private void OnItemCollected(CollectibleDefinition item)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].Item == item)
                {
                    slots[i].SetCollected(true, true);
                }
            }
        }

        private void OnMissingItemsRequested(IReadOnlyList<CollectibleDefinition> items)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                CollectibleDefinition item = slots[i].Item;
                if (!goal.IsCollected(item) && Contains(items, item))
                {
                    slots[i].FlashMissing();
                }
            }
        }

        private static bool Contains(IReadOnlyList<CollectibleDefinition> items, CollectibleDefinition item)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] == item)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
