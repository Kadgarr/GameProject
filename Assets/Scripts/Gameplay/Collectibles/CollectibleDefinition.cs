using UnityEngine;

namespace DarknessInside.Gameplay.Collectibles
{
    /// <summary>Описание предмета, который нужно собрать на уровне: название и иконка для HUD.</summary>
    [CreateAssetMenu(fileName = "Collectible", menuName = "Darkness Inside/Collectible")]
    public sealed class CollectibleDefinition : ScriptableObject
    {
        [SerializeField] private string displayName;
        [SerializeField] private Sprite icon;

        public string DisplayName => displayName;
        public Sprite Icon => icon;
    }
}
