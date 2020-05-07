using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scale : MonoBehaviour
{
    public Image fillingScale;

    public GameObject diePlayer;
    public float filling = 0.0001f;

    public DeadPlayer Активує
    {
        get => default;
        set
        {
        }
    }

    private void Start()
    {
        
    }

    void Update()
    {
        fillingScale.fillAmount -= filling;

        if (fillingScale.fillAmount == 0)
        {
            DeadPlayer dead = diePlayer.GetComponent<DeadPlayer>();
            dead.StatePlayer(true);
            this.GetComponent<Die>().Check = true;
        }
    }
}
