using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class PressButtonRiddle2 : MonoBehaviour
{
    public Sprite switchOnBut;
    public Sprite switchOffBut;

    public float timer;
    public bool stateOFdoor=false;
  
    Riddle2Code riddle;

    private void Start()
        {
            riddle = this.GetComponent<Riddle2Code>();
        }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GetComponentInChildren<SpriteRenderer>().sprite = switchOnBut;
            riddle.RiddleClick();
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        GetComponentInChildren<SpriteRenderer>().sprite = switchOffBut;
    }
}
