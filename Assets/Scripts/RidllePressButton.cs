using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RidllePressButton : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite switchOnBut;
    public Sprite switchOffBut;
    public int NumberForRiddle;
    public GameObject Cell1;
    public GameObject Cell2;
    public GameObject Cell3;
    public GameObject Cell4;
    public GameObject Controller;



    public bool stateOFdoor = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GetComponentInChildren<SpriteRenderer>().sprite = switchOnBut;
            if (Controller.GetComponentInChildren<RiddleController>().count==1)
            {
                Cell1.GetComponentInChildren<RiddleCode>().Code(NumberForRiddle.ToString());
                Controller.GetComponentInChildren<RiddleController>().ButtonController2(NumberForRiddle.ToString());
            }
            if (Controller.GetComponentInChildren<RiddleController>().count == 2)
            {
                Cell2.GetComponentInChildren<RiddleCode>().Code(NumberForRiddle.ToString());
                Controller.GetComponentInChildren<RiddleController>().ButtonController2(NumberForRiddle.ToString());
            }
            if (Controller.GetComponentInChildren<RiddleController>().count == 3)
            {
                Cell3.GetComponentInChildren<RiddleCode>().Code(NumberForRiddle.ToString());
                Controller.GetComponentInChildren<RiddleController>().ButtonController2(NumberForRiddle.ToString());
            }
            if (Controller.GetComponentInChildren<RiddleController>().count == 4)
            {
                Cell4.GetComponentInChildren<RiddleCode>().Code(NumberForRiddle.ToString());
                Controller.GetComponentInChildren<RiddleController>().ButtonController2(NumberForRiddle.ToString());
            }
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        Controller.GetComponentInChildren<RiddleController>().ButtonController1();
        GetComponentInChildren<SpriteRenderer>().sprite = switchOffBut;
       
    }
}
