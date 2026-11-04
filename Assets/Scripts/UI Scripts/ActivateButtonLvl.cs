using DarknessInside.Core.Save;
using DarknessInside.Dev;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Открывает кнопки уровней в меню по прогрессу из SaveService.
/// В режиме разработчика (DeveloperSettings) открыты все уровни.
/// </summary>
public class ActivateButtonLvl : MonoBehaviour
{
    [Tooltip("Кнопки уровней по порядку: первая — уровень 1.")]
    [SerializeField] private Button[] levelButtons = new Button[0];
    [SerializeField] private DeveloperSettings developerSettings;

    private void OnEnable()
    {
        Refresh(SaveService.Instance.LastCompletedLevel);
        SaveService.Instance.ProgressChanged += Refresh;
    }

    private void OnDisable()
    {
        SaveService.Instance.ProgressChanged -= Refresh;
    }

    private void Refresh(int lastCompletedLevel)
    {
        bool unlockAll = developerSettings != null && developerSettings.UnlockAllLevels;
        int unlocked = unlockAll ? levelButtons.Length : lastCompletedLevel + 1;
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (levelButtons[i] != null)
            {
                levelButtons[i].interactable = i < unlocked;
            }
        }
    }
}
