using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateMovePlatformScript : MonoBehaviour
{
    public GameObject Thorns;
    public void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GetComponent<PlatformMove>().enabled = true;
            Thorns.GetComponentInChildren<PlatformMove>().enabled = true;
        }
    }
}
