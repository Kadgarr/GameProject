using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPlatform : MonoBehaviour
{
    private bool Check=false;
    public float timer;

    private void Update()
    {
        if (Check == true)
        {
           
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                print(true);
            }
            if (timer <= 0)
            {
                timer = 0.3f;
               
                this.GetComponent<Rigidbody2D>().gravityScale = 2;
                this.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Check = true;

        }
    }
}
