using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    public Item Містить
    {
        get => default;
        set
        {
        }
    }
}

[System.Serializable]

public class Item
{
    public int Id;
    public string Name;
    public Sprite Image;

    public AddItemPlAYER Підбирається
    {
        get => default;
        set
        {
        }
    }
}