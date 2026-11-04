using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorOpen : MonoBehaviour
{

    public Sprite doorOpened;

    public CameraController1 Вмикає
    {
        get => default;
        set
        {
        }
    }

    public void OpenedDoor(bool state)
    {

        if (state)
        {
            Open();
        }
    }

    public void OpenedDoorClick()
    {
        Open();
    }

    private void Open()
    {
        var doorCollider = GetComponentInChildren<BoxCollider2D>();
        if (doorCollider != null)
        {
            doorCollider.enabled = false;
        }

        var doorRenderer = GetComponentInChildren<SpriteRenderer>();
        if (doorRenderer != null && doorOpened != null)
        {
            doorRenderer.sprite = doorOpened;
        }
    }

    public void OpenDoorClickUp(Button button)
    {
        if (button != null)
        {
            button.interactable = false;
        }
    }
}
