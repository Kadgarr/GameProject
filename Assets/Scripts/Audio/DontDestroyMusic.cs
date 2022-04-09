using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class DontDestroyMusic : MonoBehaviour
{
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");

        
        if (SceneManager.GetActiveScene().name == "EndScene")
        {
            Destroy(objs[0]);

        }
        else
        {
            
            if (objs.Length > 1)
            {
                Destroy(this.gameObject);
            }
            else
                DontDestroyOnLoad(this.gameObject);
        }
        



       
    }
}
