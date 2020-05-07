using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorScript : MonoBehaviour
{
    
    public List<ColoredElement> elements = new List<ColoredElement>();

    private float count =1;

    public float fillingCount = 0.0001f;

    public ColoredElement Фарбує
    {
        get => default;
        set
        {
        }
    }

    private void Start()
    {
        foreach(ColoredElement x in elements)
        {
            x.element.GetComponent<Image>().color = new Color(0, 0, 0, 1);
        }
         
    }

    private void Update()
    {
        count -= fillingCount;
        foreach (ColoredElement x in elements)
        {
            x.element.GetComponent<Image>().color = new Color(1, 1,1, count);
        }
    }
}

[System.Serializable]
public class ColoredElement
{
    public GameObject element;
}
