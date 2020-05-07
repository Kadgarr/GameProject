using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;


public class FastClick : MonoBehaviour
{
    public Image LoadingCircle;
    public Image ComplitedCircle;
    public Sprite ArmSpriteOn;
    public Button ClickButton;

    public GameObject ObjectDoor;
    public GameObject canvasBut;

    public SpriteRenderer ArmSprite;

    private float minus = 0.05f;

    bool checkForFillAmount = false;
    public bool checkForDesrtItem = false;
   
    void FixedUpdate()
    {
        if (LoadingCircle.fillAmount > 0)
        {
            if ( Time.frameCount % 2 ==0)
            {
                LoadingCircle.fillAmount -= minus;
            }
        }

        if (LoadingCircle.fillAmount == 1)
        {
            checkForDesrtItem = true;
            ComplitedCircle.enabled = true;
            ClickButton.image = ComplitedCircle;
            minus = 0;
            ArmSprite.sprite = ArmSpriteOn;
            ObjectDoor.GetComponentInChildren<DoorOpen>().OpenedDoor(true);
            ClickButton.interactable = false;
        }
        if (checkForDesrtItem)
            canvasBut.gameObject.SetActive(false);


    }


    public void ButtonFillClick()
    {
        LoadingCircle.fillAmount += 0.3f;
    }
}
