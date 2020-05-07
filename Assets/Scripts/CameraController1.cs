using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController1 : MonoBehaviour
{
   
    private Vector2 startpos;

    

    public float posY;
    public GameObject camObj;

    private Camera cam;
    
    float pos;

     private void Start()
     {
        cam = camObj.GetComponent<Camera>();
        startpos = cam.ScreenToWorldPoint(Input.mousePosition);
        
      }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            cam.transform.position = new Vector3(transform.position.x, posY, transform.position.z );
        }

        
    }
        
           
     
}
