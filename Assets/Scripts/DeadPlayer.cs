using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeadPlayer : MonoBehaviour
{
    public bool stateForTimer = false;
    public Sprite SpriteDead;
    private Animator Anim;

    public GameObject ButRight;
    public GameObject ButLeft;
    public GameObject ButUp;

    public ActivateMovePlatformScript ActivateMovePlatformScript
    {
        get => default;
        set
        {
        }
    }

    public void StatePlayer(bool state)
    {
        if (state == true)
        {
            Anim = GetComponent<Animator>();
            Anim.enabled = false;
            GetComponentInChildren<SpriteRenderer>().sprite = SpriteDead;
            
            /*ButRight.GetComponentInChildren<EventTrigger>().enabled = false;
            ButLeft.GetComponentInChildren<EventTrigger>().enabled = false;*/
            ButUp.GetComponent<EventTrigger>().enabled = false;
            stateForTimer = true;
            try
            {
                GetComponent<AccelerContr>().enabled = false;
            }
            catch
            {

            }
            GetComponent<PlayerController>().enabled = false;
        }
    }
}
