using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using System.Threading;

public class ShowUpTurns : MonoBehaviour
{
    public float step;

    private float progress;

    private float progress2;

    private int counter=0;

    public bool Check;

    public float second;

    public bool GlobalCheck;

    public Vector3 posYUp;

    public Vector3 posYDown;

    private Vector3 startPos;
    
    public Vector3 CheckPosition;



    // Update is called once per frame

    private void Start()
    {
        CheckPosition = transform.position;
        
    }

    
    void FixedUpdate()
    {
        CheckPosition = transform.position;
        
            if (CheckPosition == posYUp )
            {
                if (DateTime.Now.Second % second == 0 )
                {
                    Check = true;
                    progress2 = 0;
                }
                
            }
            else if (CheckPosition == posYDown)
            {
                if (DateTime.Now.Second % second == 0)
                {
                    Check = false;
                    progress = 0;
                }
                
            }
            
            if (Check && progress2==0)
            {
                transform.position = Vector3.Lerp(posYUp, posYDown, progress);
                progress += step;

            }
            else if(!Check && progress==0)
            {
                transform.position = Vector3.Lerp(posYDown, posYUp, progress2);
                progress2 += step;
            }
        
    }
    
}
