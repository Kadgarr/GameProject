using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    
    private string currentLanguage;
    private Dictionary<string, string> localizedText;
    public static bool isReady = false;

    public delegate void ChangeLangText();
    public event ChangeLangText OnLanguageChanged;


     void Awake()
     {
         if (!PlayerPrefs.HasKey("Language"))
         {
             if (Application.systemLanguage == SystemLanguage.Russian)
             {
                 PlayerPrefs.SetString("Language", "ru_RU");
             }
             else
             if ( Application.systemLanguage == SystemLanguage.Ukrainian)
             {
                 PlayerPrefs.SetString("Language", "ua_UA");
             }
            else
            {
                PlayerPrefs.SetString("Language", "en_US");
            }
         }
         currentLanguage = PlayerPrefs.GetString("Language");

         LoadLocalizedText(currentLanguage);
     }

    public void LoadLocalizedText(string langName)
    {
        string path = Application.streamingAssetsPath + "/Languages/" + langName + ".json";
        localizedText = new Dictionary<string, string>();
        string dataAsJson;

        if (Application.platform == RuntimePlatform.Android)
        {
            WWW reader = new WWW(path);
            while (!reader.isDone) { }

            dataAsJson = reader.text;
        }
        else
        {
            dataAsJson = File.ReadAllText(path);
        }

        LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

        
        for (int i = 0; i < loadedData.items.Length; i++)
        {
            localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
        }

        PlayerPrefs.SetString("Language", langName);
        currentLanguage = PlayerPrefs.GetString("Language");
        isReady = true;
       Debug.Log("Data loaded, dictionary contains: " + localizedText.Count + " entries");
         OnLanguageChanged?.Invoke();
    }

    public string GetLocalizedValue(string key)
    {
        if (localizedText.ContainsKey(key))
        {
            return localizedText[key];
        }
        else
        {
            throw new Exception("Localized text with key \"" + key + "\" not found");
        }
    }

    public string CurrentLanguage
    {
        get
        {
            return currentLanguage;
        }
        set
        {
            PlayerPrefs.SetString("Language", value);
            currentLanguage = PlayerPrefs.GetString("Language");
        }
    }

    public bool IsReady
    {
        get
        {
            return isReady;
        }
         
    }

    public LocalizationData Локалізує
    {
        get => default;
        set
        {
        }
    }
}