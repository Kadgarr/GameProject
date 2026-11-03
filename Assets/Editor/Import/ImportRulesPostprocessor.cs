using UnityEditor;

namespace DarknessInside.Editor.Import
{
    /// <summary>
    /// Применяет ImportRules только к НОВЫМ файлам (при первом импорте).
    /// Ручные правки настроек существующих ассетов не перетираются.
    /// </summary>
    public sealed class ImportRulesPostprocessor : AssetPostprocessor
    {
        private void OnPreprocessTexture()
        {
            if (!assetImporter.importSettingsMissing)
            {
                return;
            }

            ImportRules rules = ImportRules.Load();
            ImportRules.TextureRule rule = rules != null ? rules.FindTextureRule(assetPath) : null;
            if (rule != null)
            {
                ImportRules.Apply((TextureImporter)assetImporter, rule);
            }
        }

        private void OnPreprocessAudio()
        {
            if (!assetImporter.importSettingsMissing)
            {
                return;
            }

            ImportRules rules = ImportRules.Load();
            ImportRules.AudioRule rule = rules != null ? rules.FindAudioRule(assetPath) : null;
            if (rule != null)
            {
                ImportRules.Apply((AudioImporter)assetImporter, rule);
            }
        }
    }
}
