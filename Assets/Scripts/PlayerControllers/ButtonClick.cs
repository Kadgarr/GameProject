using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        if(gameObject.name== "ButtonLeft")
        {
            GameObject.Find("Sprite").GetComponent<PlayerController>().horizontalspeed = -1;
            
        }
        else
        if(gameObject.name == "ButtonRight")
        {
            GameObject.Find("Sprite").GetComponent<PlayerController>().horizontalspeed = 1;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameObject.Find("Sprite").GetComponent<PlayerController>().horizontalspeed = 0;
    }

}
