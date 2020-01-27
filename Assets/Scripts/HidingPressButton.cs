using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingPressButton : MonoBehaviour
{
    public Sprite switchOnBut;
    public Sprite switchOffBut;

    public GameObject Wall1;
    public GameObject Wall2;
    public GameObject Wall3;
    public GameObject Wall4;
    public GameObject ColliderWall;
    public GameObject ColliderGround;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GetComponentInChildren<SpriteRenderer>().sprite = switchOnBut;
            ColliderWall.GetComponentInChildren<BoxCollider2D>().enabled = false;
            ColliderGround.GetComponentInChildren<BoxCollider2D>().enabled = true;
            Wall1.GetComponentInChildren<SpriteRenderer>().enabled = true;
            Wall2.GetComponentInChildren<SpriteRenderer>().enabled = true;
            Wall3.GetComponentInChildren<SpriteRenderer>().enabled = true;
            Wall4.GetComponentInChildren<SpriteRenderer>().enabled = true;
        }
    }
    public void OnCollisionExit2D(Collision2D collision)
    {
        GetComponentInChildren<SpriteRenderer>().sprite = switchOffBut;
    }
}
