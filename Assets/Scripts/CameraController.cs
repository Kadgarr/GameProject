using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    private Vector2 startpos;

    private Camera cam;

    float pos;

    public float speed;

    private float targetPos;

    private void Start()
    {
        cam = GetComponent<Camera>();

        targetPos = transform.position.y;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) startpos = cam.ScreenToWorldPoint(Input.mousePosition);
        else if (Input.GetMouseButton(0))
        {
            pos = cam.ScreenToWorldPoint(Input.mousePosition).y - startpos.y;
            targetPos = Mathf.Clamp(transform.position.y - pos, -9.68f, 0.1f);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, targetPos, speed*Time.deltaTime), transform.position.z);
    }
}
