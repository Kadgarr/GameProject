using DarknessInside.Dev;
using UnityEditor;

namespace DarknessInside.Editor.Dev
{
    public static class DeveloperMenu
    {
        private const string SettingsPath = "Assets/Settings/Developer/DeveloperSettings.asset";

        [MenuItem("Darkness Inside/Developer Settings", priority = 100)]
        private static void SelectSettings()
        {
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<DeveloperSettings>(SettingsPath);
        }
    }
}
