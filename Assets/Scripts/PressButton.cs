using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class PressButton : MonoBehaviour
{
    public Sprite switchOnBut;
    public Sprite switchOffBut;

    public float timer;
    public bool stateOFdoor=false;
    public GameObject objectd;





    //public void OnTriggerEnter2D(Collider2D Other)
    //{

    //    if (Other.CompareTag("Player") )
    //    {
    //        GetComponentInChildren<SpriteRenderer>().sprite = switchOnBut;
    //        Proverka = false;
    //        stateOFdoor = true;
    //        objectd.GetComponentInChildren<DoorOpen>().OpenedDoor(stateOFdoor);
    //    }

    //}

    /*public void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GetComponentInChildren<SpriteRenderer>().sprite = switchOnBut;
            Proverka = false;
            stateOFdoor = true;
            objectd.GetComponentInChildren<DoorOpen>().OpenedDoor(stateOFdoor);
        }
    }*/
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GetComponentInChildren<SpriteRenderer>().sprite = switchOnBut;
            
            try
            {
                stateOFdoor = true;
                objectd.GetComponentInChildren<DoorOpen>().OpenedDoor(stateOFdoor);
            }
            catch
            {

            }
            
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        GetComponentInChildren<SpriteRenderer>().sprite = switchOffBut;
    }
}
