using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiddleController : MonoBehaviour
{
    public int count=1;
    string text1;
    string text2 = "7777";
    public GameObject Door;
    bool stateOfDoor = false;

    public void ButtonController1()
    {
        count++;
    }
    public void ButtonController2(string text)
    {
        print(text1);
        text1 += text;
        if (text2 == text1)
        {
            stateOfDoor = true;
            Door.GetComponentInChildren<DoorOpen>().OpenedDoor(stateOfDoor);
        }
    }
}
