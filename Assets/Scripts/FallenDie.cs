using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class FallenDie : MonoBehaviour
{
    public float GravityScale;
    public int IndexScene;
    public float timer;
    public bool Check;
    public GameObject Player;
    bool CheckGround = false;
    private void Update()
    {
        if (Check == true)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;

            }
            if (timer <= 0)
            {
                timer = 2;
                SceneManager.LoadScene(IndexScene);
            }
        }
    }

  
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && CheckGround!=true)
        {
            print("true");
            Check = true;
            Player.GetComponentInChildren<DeadPlayer>().StatePlayer(true);
        }
        if (collision.gameObject.tag == "Ground")
        {
            CheckGround = true;

            for (int i = 0; i < 5; i++)
            {
                GetComponentsInChildren<BoxCollider2D>()[i].enabled = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            for(int i = 0; i < 5; i++)
            {
                GetComponentsInChildren<Rigidbody2D>()[i].gravityScale = GravityScale;
            }
            
        }
    }
}
