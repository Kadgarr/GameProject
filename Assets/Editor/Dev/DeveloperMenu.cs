using DarknessInside.Core.Save;
using DarknessInside.Dev;
using UnityEditor;
using UnityEngine;

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

        [MenuItem("Darkness Inside/Save/Reset Progress", priority = 110)]
        private static void ResetProgress()
        {
            SaveService.Instance.ResetProgress();
            Debug.Log("Progress reset: " + JsonFileSaveStorage.DefaultPath);
        }

        [MenuItem("Darkness Inside/Save/Delete Save File", priority = 111)]
        private static void DeleteSave()
        {
            new JsonFileSaveStorage(JsonFileSaveStorage.DefaultPath).Delete();
            SaveService.SetInstance(null);
            Debug.Log("Save file deleted: " + JsonFileSaveStorage.DefaultPath);
        }

        [MenuItem("Darkness Inside/Save/Open Save Folder", priority = 112)]
        private static void OpenSaveFolder()
        {
            EditorUtility.RevealInFinder(JsonFileSaveStorage.DefaultPath);
        }
    }
}
