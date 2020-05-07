using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LocalizationData 
{
    public LocalizationItem[] items;

    public LocalizationItem Містить
    {
        get => default;
        set
        {
        }
    }
}

[System.Serializable]
public class LocalizationItem
{
    public string key;
    public string value;
}
