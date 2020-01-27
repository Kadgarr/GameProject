﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Die : MonoBehaviour
{
    public float timer;
    public bool Check;
    public GameObject Player;

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
                timer = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Check = true;
            Player.GetComponentInChildren<DeadPlayer>().StatePlayer(true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Check = true;
            Player.GetComponentInChildren<DeadPlayer>().StatePlayer(true);
        }
    }
}
