using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrueExit : MonoBehaviour
{
    public GameObject TrueCollideer;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            TrueCollideer.GetComponentInChildren<BoxCollider2D>().enabled = false;
        }
    }


}
