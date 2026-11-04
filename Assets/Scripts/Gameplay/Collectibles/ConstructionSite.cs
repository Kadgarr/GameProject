using System.Collections.Generic;
using UnityEngine;

namespace DarknessInside.Gameplay.Collectibles
{
    /// <summary>
    /// Место постройки. Если у игрока есть все нужные предметы — показывает кнопку постройки
    /// (быстрые нажатия, FastClick). Если нет — просит HUD подсветить недостающие предметы.
    /// </summary>
    public sealed class ConstructionSite : MonoBehaviour
    {
        private const string PlayerTag = "Player";

        [SerializeField] private LevelCollectionGoal goal;
        [SerializeField] private List<CollectibleDefinition> requiredItems = new List<CollectibleDefinition>();

        [Header("Постройка")]
        [Tooltip("Канвас с кнопкой быстрых нажатий.")]
        [SerializeField] private GameObject buildPrompt;
        [Tooltip("Объект, который появляется, когда предметы собраны (например, рычаг). Необязательно.")]
        [SerializeField] private GameObject constructionObject;
        [SerializeField] private FastClick buildButton;
        [Tooltip("Триггер места постройки — отключается после постройки.")]
        [SerializeField] private Collider2D siteTrigger;

        private bool _built;

        private void Reset()
        {
            siteTrigger = GetComponent<Collider2D>();
        }

        private void OnEnable()
        {
            if (buildButton != null)
            {
                buildButton.Completed += OnBuilt;
            }
        }

        private void OnDisable()
        {
            if (buildButton != null)
            {
                buildButton.Completed -= OnBuilt;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_built || goal == null || !other.CompareTag(PlayerTag))
            {
                return;
            }

            if (goal.HasAll(requiredItems))
            {
                SetActive(buildPrompt, true);
                SetActive(constructionObject, true);
            }
            else
            {
                goal.ReportMissing(requiredItems);
            }
        }

        private void OnBuilt()
        {
            _built = true;
            if (siteTrigger != null && siteTrigger.isTrigger)
            {
                siteTrigger.enabled = false;
            }
        }

        private static void SetActive(GameObject target, bool active)
        {
            if (target != null && target.activeSelf != active)
            {
                target.SetActive(active);
            }
        }
    }
}
