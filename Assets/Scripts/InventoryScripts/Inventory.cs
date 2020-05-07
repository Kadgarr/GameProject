using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;


public class Inventory : MonoBehaviour
{
    public List<ItemInventory> items = new List<ItemInventory>();

    public GameObject BackGround;

    
    public DataBase data;

    public GameObject gameObjShow;

    public GameObject InventoryMainObject;
    public int MaxCount;

    public Camera cam;
    public EventSystem ev;

    public int currentID;
    public ItemInventory currentItem;

   //для перемещения инвентаря по экрану
    public RectTransform movingObject;
    public Vector3 offset;


    public GameObject deleteItem;
    public GameObject DeleteInventory;

    public int countForAdd=0;

    private AudioSource objSource;

    public Item Містить
    {
        get => default;
        set
        {
        }
    }

    public void Start()
    {
        if (items.Count == 0)
        {
            AddGraphics();
        }

        UpdateInventory();
        
        //тест на вместимость
       /* for(int i=0; i < MaxCount; i++)
        {
            AddItem(i, data.items[Random.Range(0, data.items.Count)]);
        }*/
    }


    public void OpenClickInventory()
    {
        BackGround.SetActive(!BackGround.activeSelf);
        objSource = GameObject.FindGameObjectWithTag("MobileControl").GetComponent<AudioSource>();

        objSource.Play();
        if (BackGround.activeSelf)
        {
            UpdateInventory();
        }
    }


    public void Update()
    {
       
        if (currentID != -1)
        {
           MoveObject();
            
        }
        else
        {
            UpdateInventory();
        }
    }

    public void AddItem(int id, Item item, int count)
    {
        items[id].id = item.Id;
        items[id].itemObject.GetComponent<Image>().sprite = item.Image;
        items[id].count = count;
         
        if (item.Id == 0)
        {
            items[id].itemObject.GetComponentInChildren<Text>().text = "";
        }
    }

    public void AddInventoryItem(int id, ItemInventory itemInv)
    {
        items[id].id = itemInv.id;
        items[id].itemObject.GetComponent<Image>().sprite = data.items[itemInv.id].Image;
        items[id].count = itemInv.count;

        if (itemInv.id == 0)
        {
            items[id].itemObject.GetComponentInChildren<Text>().text = "";
        }

       
    }

    public void RemoveItemFromInventory()
    {
        int idClick=0;
        print(items[idClick].id.ToString());
        idClick = int.Parse(deleteItem.name);
        items.Remove(items[idClick]);
        
    }

    public void AddGraphics()
    {
        for(int i=0;i<MaxCount; i++)
        {
            //для перемещения предметов в инвентаре
            GameObject newItem = Instantiate(gameObjShow, InventoryMainObject.transform) as GameObject;

            //для отображения количества ячеек для предметов в инвентаре
            newItem.name = i.ToString();

            ItemInventory ii = new ItemInventory();
            ii.itemObject = newItem;

        
            // определяем положение и размеры ячеек 
            RectTransform rt = newItem.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(0, 0, 0);
            rt.localScale = new Vector3(1, 1, 1);

            newItem.GetComponentInChildren<RectTransform>().localScale = new Vector3(1, 1, 1);

            //назначаем каждый пункт инвентаря кнопкой
         

            Button tempButton = newItem.GetComponent<Button>();
            
            tempButton.onClick.AddListener(delegate { SelectObject(); });
            
            items.Add(ii);           
        }
    }

    public void UpdateInventory()
    {
        for (int i=0; i < MaxCount; i++)
        {
            if (items[i].id!=0 && items[i].count > 1)
            {
                items[i].itemObject.GetComponentInChildren<Text>().text = items[i].count.ToString();
            }
            else
            {
                items[i].itemObject.GetComponentInChildren<Text>().text = "";
            }

          // items[i].itemObject.GetComponent<Image>().sprite = data.items[items[i].id].Image;
        }

    }

    public void SelectObject()
    {
        if (currentID == -1)
        {
            currentID = int.Parse(ev.currentSelectedGameObject.name);
            currentItem = CopeInventoryItem(items[currentID]);

            if (currentItem.id != 0)
            {
                movingObject.gameObject.SetActive(true);
                movingObject.GetComponent<Image>().sprite = data.items[currentItem.id].Image;
                AddItem(currentID, data.items[0],0);
            }
            
        }
        else
        {
            ItemInventory II = items[int.Parse(ev.currentSelectedGameObject.name)];
           
            AddInventoryItem(currentID, II);

            AddInventoryItem(int.Parse(ev.currentSelectedGameObject.name), currentItem);

            currentID = -1;
            movingObject.gameObject.SetActive(false);
        }


    } 

    //перемещение по экрану
   public void MoveObject()
   {
        Vector3 pos = Input.mousePosition + offset;
        pos.z = InventoryMainObject.GetComponent<RectTransform>().position.z;
        movingObject.position = cam.ScreenToWorldPoint(pos);
   }

    //перемещение по инвентарю
    public ItemInventory CopeInventoryItem(ItemInventory old)
    {
        ItemInventory New = new ItemInventory();
        New.id = old.id;
        New.itemObject = old.itemObject;
        return New;
    }

    

}

[System.Serializable]

public class ItemInventory
{
    public int id;
    public GameObject itemObject;
    public int count;
}
