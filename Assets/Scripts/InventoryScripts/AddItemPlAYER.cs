using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddItemPlAYER : MonoBehaviour
{
     public  GameObject inventory;
     int i = 0;

    public ItemInventory Додається_до
    {
        get => default;
        set
        {
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Inventory INV = inventory.GetComponentInChildren<Inventory>();


        if (collision.gameObject.tag == "Player" && this.gameObject.tag=="PartOfArmStick")
        {
            
            for (int i=0; i < INV.MaxCount; i++)
            {
                if (INV.items[i].count ==0)
                {
                    INV.AddItem(i, INV.data.items[1],1);
                    break;
                }
            }
            Destroy(this.gameObject);
           
        }
        if (collision.gameObject.tag == "Player" && this.gameObject.tag == "PartOfArmOwn")
        {
            for (int i = 0; i < INV.MaxCount; i++)
            {
                if (INV.items[i].count == 0)
                {
                    INV.AddItem(i, INV.data.items[2],1);
                    break;
                }
            }
            Destroy(this.gameObject);
          
        }

        if (collision.gameObject.tag == "Player" && this.gameObject.tag == "CircleArm")
        {

            for (int i = 0; i < INV.MaxCount; i++)
            {
                if (INV.items[i].count == 0)
                {
                    INV.AddItem(i, INV.data.items[3], 1);
                    break;
                }
            }
            Destroy(this.gameObject);

        }
    }

    
}
