using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPlatform : MonoBehaviour
{
    private bool Check=false;
    public float timer;

    public FallenDie Вмирає_від
    {
        get => default;
        set
        {
        }
    }

    private void Update()
    {
        if (Check == true)
        {
           
            if (timer <= 0)
            {
                timer = 0.3f;
                GetComponent<Rigidbody2D>().isKinematic = false;
                this.GetComponent<Rigidbody2D>().gravityScale = 2;
                this.GetComponent<BoxCollider2D>().enabled = false;
            }
            else 
            {
                timer -= Time.deltaTime;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Check = true;
        }
    }
}
