using DarknessInside.Core.Localization;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Текст UI по ключу локализации. Обновляется при смене языка через событие.</summary>
[RequireComponent(typeof(Text))]
public class LocalizedText : MonoBehaviour
{
    [SerializeField] private string key;

    private Text _text;

    private void Awake()
    {
        _text = GetComponent<Text>();
    }

    private void OnEnable()
    {
        Apply();
        LocalizationService.LanguageChanged += Apply;
    }

    private void OnDisable()
    {
        LocalizationService.LanguageChanged -= Apply;
    }

    private void Apply()
    {
        LocalizationService service = LocalizationService.Instance;
        if (service != null && _text != null)
        {
            _text.text = service.Get(key);
        }
    }
}
