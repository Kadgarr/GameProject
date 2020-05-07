using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Riddle2Code : MonoBehaviour
{
    public int number;
    public int numberOfGroupButtons;

    public GameObject objCanv;


    private bool check;
    private bool check2;

    public GameObject cell1;
    public GameObject cell2;
    public GameObject cell3;
    public GameObject doorObj;

    DoorOpen door;
    SummRiddle2 summ;

    Text cell_1;
    Text cell_2;
    Text cell_3;

    private void Start()
    {
        cell_1 = cell1.GetComponent<Text>();
        cell_2 = cell2.GetComponent<Text>();
        cell_3 = cell3.GetComponent<Text>();
        door = doorObj.GetComponent<DoorOpen>();
        summ = objCanv.GetComponent<SummRiddle2>();
    }

    public void RiddleClick()
    {
        if(numberOfGroupButtons == 1)
        {
            if (check)
            {
                summ.summ1 -= number;
                check = false;
            }
            else
            {
                summ.summ1 += number;
                check = true;
            }
            Translate(1);
        }
        else
        if (numberOfGroupButtons == 2)
        {
            if (check2)
            {
                summ.summ2 -= number;
                check2 = false;
            }
            else
            {
                summ.summ2 += number;
                check2 = true;
            }
            Translate(2);
        }
    }

    public void Translate(int check)
    {
        if (check == 1)
            cell_1.text = summ.summ1.ToString();
        else if (check == 2)
            cell_2.text = summ.summ2.ToString();
        cell_3.text = (summ.summ1 - summ.summ2).ToString();
        if ((summ.summ1 - summ.summ2) == 8)
        {
            door.GetComponent<DoorOpen>().OpenedDoor(true);
        }
    }
}
