using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using DarknessInside.Dev;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Открывает кнопки уровней в меню по сохранённому прогрессу.
/// В режиме разработчика (DeveloperSettings) открывает все уровни.
/// Полная переработка — на этапе 3 (LevelDatabase + SaveService).
/// </summary>
public class ActivateButtonLvl : MonoBehaviour
{
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button button5;
    public Button button6;
    public Button button7;
    public Button button8;
    public Button button9;
    public Button button10;
    public Button button11;
    public Button button12;
    public Button button13;
    public Button button14;
    public Button button15;

    [SerializeField] private DeveloperSettings developerSettings;

    private void Start()
    {
        Button[] buttons =
        {
            button1, button2, button3, button4, button5,
            button6, button7, button8, button9, button10,
            button11, button12, button13, button14, button15
        };

        if (developerSettings != null && developerSettings.UnlockAllLevels)
        {
            Unlock(buttons, buttons.Length);
            return;
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        string path = Path.Combine(Application.persistentDataPath, "level_completed");
#else
        string path = Path.Combine(Application.dataPath, "level_completed");
#endif
        if (!File.Exists(path))
        {
            Unlock(buttons, 1);
            return;
        }

        List<IndexScene> completedScenes;
        var formatter = new BinaryFormatter();
        using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            completedScenes = (List<IndexScene>)formatter.Deserialize(stream);
        }

        // Открыты все уровни до последнего пройденного включительно и следующий за ним.
        int lastCompleted = 0;
        foreach (IndexScene scene in completedScenes)
        {
            if (scene.Index > lastCompleted)
            {
                lastCompleted = scene.Index;
            }
        }

        Unlock(buttons, lastCompleted + 1);
    }

    private static void Unlock(Button[] buttons, int count)
    {
        int last = Mathf.Min(count, buttons.Length);
        for (int i = 0; i < last; i++)
        {
            if (buttons[i] != null)
            {
                buttons[i].interactable = true;
            }
        }
    }
}
