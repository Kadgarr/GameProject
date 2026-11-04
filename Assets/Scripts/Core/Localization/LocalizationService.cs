using System;
using System.Collections.Generic;
using DarknessInside.Core.Save;
using UnityEngine;

namespace DarknessInside.Core.Localization
{
    /// <summary>
    /// Тексты на выбранном языке. Таблицы — JSON-файлы (TextAsset) в префабе Core: читаются сразу,
    /// без StreamingAssets и UnityWebRequest. Выбранный язык хранится в SaveService.
    /// Тексты на экране обновляются по событию LanguageChanged.
    /// </summary>
    public sealed class LocalizationService : MonoBehaviour
    {
        [Serializable]
        public sealed class Language
        {
            [Tooltip("Код языка: ru_RU, ua_UA, en_US.")]
            public string code;
            public TextAsset table;
        }

        [SerializeField] private List<Language> languages = new List<Language>();
        [SerializeField] private string fallbackCode = "en_US";

        private readonly Dictionary<string, string> _texts = new Dictionary<string, string>();

        public static LocalizationService Instance { get; private set; }

        /// <summary>Язык сменился — тексты нужно перерисовать.</summary>
        public static event Action LanguageChanged;

        public string CurrentLanguage { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            string saved = SaveService.Instance.Language;
            Load(string.IsNullOrEmpty(saved) ? DetectSystemLanguage() : saved);
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        public void SetLanguage(string code)
        {
            if (code == CurrentLanguage)
            {
                return;
            }

            Load(code);
            SaveService.Instance.SetLanguage(CurrentLanguage);
            LanguageChanged?.Invoke();
        }

        /// <summary>Текст по ключу; если ключа нет — сам ключ (видно на экране, но игра не падает).</summary>
        public string Get(string key)
        {
            string value;
            if (key != null && _texts.TryGetValue(key, out value))
            {
                return value;
            }

            Debug.LogWarning("Localization: key '" + key + "' not found for " + CurrentLanguage);
            return key;
        }

        private void Load(string code)
        {
            Language language = Find(code) ?? Find(fallbackCode);
            _texts.Clear();
            if (language == null || language.table == null)
            {
                Debug.LogError("Localization: no table for '" + code + "'.");
                return;
            }

            CurrentLanguage = language.code;
            LocalizationData data = JsonUtility.FromJson<LocalizationData>(language.table.text);
            if (data == null || data.items == null)
            {
                return;
            }

            for (int i = 0; i < data.items.Length; i++)
            {
                _texts[data.items[i].key] = data.items[i].value;
            }
        }

        private Language Find(string code)
        {
            for (int i = 0; i < languages.Count; i++)
            {
                if (languages[i].code == code)
                {
                    return languages[i];
                }
            }

            return null;
        }

        private static string DetectSystemLanguage()
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.Russian:
                    return "ru_RU";
                case SystemLanguage.Ukrainian:
                    return "ua_UA";
                default:
                    return "en_US";
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            LanguageChanged = null;
        }
    }
}
