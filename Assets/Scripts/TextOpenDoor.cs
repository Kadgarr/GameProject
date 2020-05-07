using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextOpenDoor : MonoBehaviour
{
    DoorOpen door;
    public GameObject doorClosed;

    public DoorOpen Відкриває
    {
        get => default;
        set
        {
        }
    }

    private void Start()
    {
        door = doorClosed.GetComponent<DoorOpen>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            door.OpenedDoor(true);
        }
    }

    
}
