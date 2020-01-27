using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Thorns : MonoBehaviour
{
    // Start is called before the first frame update
    public int IndexScene;
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
                timer = 2;
                SceneManager.LoadScene(IndexScene); 
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
}
