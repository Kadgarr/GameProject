using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadPlayer : MonoBehaviour
{
    public bool stateForTimer = false;
    public Sprite SpriteDead;
    private Animator Anim;
   
    public void StatePlayer(bool state)
    {
        if (state == true)
        {
            Anim = GetComponent<Animator>();
            Anim.enabled = false;
            GetComponentInChildren<SpriteRenderer>().sprite = SpriteDead;
            GetComponent<PlayerController>().enabled = false;
            stateForTimer = true;
        }
    }
}
