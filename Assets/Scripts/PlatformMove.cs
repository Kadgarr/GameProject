using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    public float timer = 0;
    public float SpeedOfPlatform = 3f;
    public Vector3 direction;
    public int IntOfDirection;
    public float time;
    public bool DirectionUp = false;

    void Start()
    {
        if (!DirectionUp)
            direction = transform.right * IntOfDirection;
        if (DirectionUp)
            direction = transform.up * IntOfDirection;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= time)
        {
            direction *= -1;
            timer = 0;
        }
        Move();
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, SpeedOfPlatform * Time.deltaTime);
    }
}
