using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiddleCode : MonoBehaviour
{
    public bool stateOfDoor;
    public GameObject Door;
    public Sprite Cell3;
    public Sprite Cell4;
    public Sprite Cell5;
    public Sprite Cell7;
    string text1;
    string text2="7543";

    public void Code(string text)
    {
        if (text1 != text )
        {
            if (text == "3")
            {
                GetComponentInChildren<SpriteRenderer>().sprite = Cell3;
                text1 += text;
               
            }
            if (text == "4")
            {
                GetComponentInChildren<SpriteRenderer>().sprite = Cell4;
                text1 += text;

            }
            if (text == "5")
            {
                this.GetComponentInChildren<SpriteRenderer>().sprite = Cell5;
                text1 += text;
          
            }
            if (text == "7")
            {
                this.GetComponentInChildren<SpriteRenderer>().sprite = Cell7;
                text1 += text;
         
            }
         
            
        }
        
        
    }
    
}
