using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelArm : MonoBehaviour
{
    public GameObject CanvasButton;

    public ConstrctPlace Збирається_на
    {
        get => default;
        set
        {
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            CanvasButton.SetActive(true);
            
        }
    }

   
   
}
