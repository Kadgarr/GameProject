using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstrctPlace : MonoBehaviour
{
    public GameObject inventory;

    public GameObject canvasCircleButton;

    public Sprite emptyCell;

    public GameObject Click;

    public GameObject constructionStuff;

    int Counter=0;

    public int countItemInv;

    public int borderCount;

    private void Update()
    {
        if (Click.GetComponent<FastClick>().checkForDesrtItem==true)
        {
            DisableItems();
            Click.GetComponent<FastClick>().checkForDesrtItem = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Inventory inv = inventory.GetComponent<Inventory>();

        if (collision.gameObject.tag == "Player")
        {
            if (countItemInv >1)
            {
                for (int i = 0; i < inv.MaxCount; i++)
                {
                    if (inv.items[i].count > 0 && (inv.items[i].id==inv.data.items[1].Id || inv.items[i].id == inv.data.items[2].Id))
                    {
                        Counter++;
                    }
                }
                if (Counter >= countItemInv)
                {
                    canvasCircleButton.gameObject.SetActive(true);

                    try
                    {
                        constructionStuff.gameObject.SetActive(true);
                    }
                    catch
                    {

                    }
                    
                }
            }
            else
            {
                for (int i = 0; i < inv.MaxCount; i++)
                {
                    if (inv.items[i].count > 0)
                    {
                        Counter++;
                    }
                }
                if (Counter >= countItemInv)
                {   
                    canvasCircleButton.gameObject.SetActive(true);

                    try
                    {
                        constructionStuff.gameObject.SetActive(true);
                    }
                    catch
                    {

                    }
                    this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                }
            }
            
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        Counter=0;
    }

    public void DisableItems()
    {
        Inventory inv = inventory.GetComponent<Inventory>();

        for (int i = 0; i < inv.MaxCount; i++)
        {
            if (inv.items[i].count > 0)
            {
                inv.items[i].itemObject.GetComponent<Image>().sprite = inv.data.items[0].Image;

                inv.items[i].count--;

                inv.items[i].id= inv.data.items[0].Id;
               
            }
        }

        
    }
}
