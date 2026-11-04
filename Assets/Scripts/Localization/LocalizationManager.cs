using DarknessInside.Core.Localization;
using UnityEngine;

/// <summary>
/// Кнопки выбора языка в меню: LoadLocalizedText("ru_RU") и т.п. Язык хранит и применяет LocalizationService.
/// </summary>
public class LocalizationManager : MonoBehaviour
{
    public void LoadLocalizedText(string langName)
    {
        if (LocalizationService.Instance != null)
        {
            LocalizationService.Instance.SetLanguage(langName);
        }
    }
}
