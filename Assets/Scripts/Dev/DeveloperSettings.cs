using UnityEngine;

namespace DarknessInside.Dev
{
    /// <summary>
    /// Настройки режима разработчика.
    /// Действуют только в редакторе и в Development Build: в релизной сборке
    /// Debug.isDebugBuild == false, поэтому все флаги автоматически игнорируются.
    /// </summary>
    [CreateAssetMenu(fileName = "DeveloperSettings", menuName = "Darkness Inside/Developer Settings")]
    public sealed class DeveloperSettings : ScriptableObject
    {
        [Tooltip("Открыть все уровни в меню выбора уровней. Только редактор и Development Build.")]
        [SerializeField] private bool unlockAllLevels = true;

        public bool UnlockAllLevels => unlockAllLevels && Debug.isDebugBuild;
    }
}
