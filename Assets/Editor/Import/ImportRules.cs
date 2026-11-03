using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DarknessInside.Editor.Import
{
    /// <summary>
    /// Правила импорта текстур и аудио по папкам. Правится в инспекторе
    /// (Assets/Settings/Import/ImportRules). Для пути выбирается правило с самым длинным совпадающим префиксом.
    /// Новые файлы получают настройки автоматически, существующие — кнопкой в инспекторе.
    /// </summary>
    [CreateAssetMenu(fileName = "ImportRules", menuName = "Darkness Inside/Import Rules")]
    public sealed class ImportRules : ScriptableObject
    {
        public const string AssetPath = "Assets/Settings/Import/ImportRules.asset";

        [Serializable]
        public sealed class TextureRule
        {
            public string name;
            [Tooltip("Папка или файл (префикс пути), например Assets/Art/Background")]
            public string pathPrefix;
            [Tooltip("Генерировать mipmaps. Для 2D-спрайтов и UI обычно выключено: mipmaps добавляют ~33% памяти.")]
            public bool generateMipmaps;
            [Tooltip("Максимальный размер на Android")]
            public int androidMaxSize = 2048;
            public TextureImporterFormat androidFormat = TextureImporterFormat.ASTC_6x6;
        }

        [Serializable]
        public sealed class AudioRule
        {
            public string name;
            [Tooltip("Папка или файл (префикс пути), например Assets/Sounds/BackGround")]
            public string pathPrefix;
            public AudioClipLoadType loadType = AudioClipLoadType.DecompressOnLoad;
            public AudioCompressionFormat compressionFormat = AudioCompressionFormat.ADPCM;
            [Range(0.01f, 1f)] public float quality = 0.5f;
            public bool forceToMono = true;
            public bool preloadAudioData = true;
            public bool loadInBackground;
        }

        [SerializeField] private List<TextureRule> textureRules = new List<TextureRule>();
        [SerializeField] private List<AudioRule> audioRules = new List<AudioRule>();

        public TextureRule FindTextureRule(string assetPath) => FindBestMatch(textureRules, assetPath, r => r.pathPrefix);

        public AudioRule FindAudioRule(string assetPath) => FindBestMatch(audioRules, assetPath, r => r.pathPrefix);

        public static ImportRules Load() => AssetDatabase.LoadAssetAtPath<ImportRules>(AssetPath);

        private static T FindBestMatch<T>(List<T> rules, string assetPath, Func<T, string> prefixOf) where T : class
        {
            T best = null;
            int bestLength = -1;
            for (int i = 0; i < rules.Count; i++)
            {
                string prefix = prefixOf(rules[i]);
                if (string.IsNullOrEmpty(prefix) || prefix.Length <= bestLength)
                {
                    continue;
                }

                if (assetPath.StartsWith(prefix, StringComparison.Ordinal))
                {
                    best = rules[i];
                    bestLength = prefix.Length;
                }
            }

            return best;
        }

        /// <summary>Применяет правило к импортёру текстуры. Возвращает true, если что-то изменилось.</summary>
        public static bool Apply(TextureImporter importer, TextureRule rule)
        {
            bool changed = false;

            if (importer.mipmapEnabled != rule.generateMipmaps)
            {
                importer.mipmapEnabled = rule.generateMipmaps;
                changed = true;
            }

            TextureImporterPlatformSettings android = importer.GetPlatformTextureSettings("Android");
            if (!android.overridden || android.maxTextureSize != rule.androidMaxSize || android.format != rule.androidFormat)
            {
                android.overridden = true;
                android.maxTextureSize = rule.androidMaxSize;
                android.format = rule.androidFormat;
                importer.SetPlatformTextureSettings(android);
                changed = true;
            }

            return changed;
        }

        /// <summary>Применяет правило к импортёру аудио. Возвращает true, если что-то изменилось.</summary>
        public static bool Apply(AudioImporter importer, AudioRule rule)
        {
            bool changed = false;
            AudioImporterSampleSettings settings = importer.defaultSampleSettings;

            if (settings.loadType != rule.loadType || settings.compressionFormat != rule.compressionFormat
                || !Mathf.Approximately(settings.quality, rule.quality) || settings.preloadAudioData != rule.preloadAudioData)
            {
                settings.loadType = rule.loadType;
                settings.compressionFormat = rule.compressionFormat;
                settings.quality = rule.quality;
                settings.preloadAudioData = rule.preloadAudioData;
                importer.defaultSampleSettings = settings;
                changed = true;
            }

            if (importer.forceToMono != rule.forceToMono)
            {
                importer.forceToMono = rule.forceToMono;
                changed = true;
            }

            if (importer.loadInBackground != rule.loadInBackground)
            {
                importer.loadInBackground = rule.loadInBackground;
                changed = true;
            }

            return changed;
        }
    }
}
