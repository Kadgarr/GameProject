using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorOpen : MonoBehaviour
{

    public Sprite doorOpened;

    public void OpenedDoor(bool state)
    {

        if (state == true)
        {
            GetComponentInChildren<BoxCollider2D>().enabled = false;
            GetComponentInChildren<SpriteRenderer>().sprite = doorOpened;
        }
    }

    public void OpenedDoorClick()
    {
        GetComponentInChildren<BoxCollider2D>().enabled = false;
        GetComponentInChildren<SpriteRenderer>().sprite = doorOpened;
    }

    public void OpenDoorClickUp(Button button)
    {
        button.GetComponent<Button>().interactable = false;
    }
}
