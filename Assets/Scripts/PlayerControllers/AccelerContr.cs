using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerContr : MonoBehaviour
{
    public float Speed;
    public float horizontalspeed;
    public float JumpForce;
    
    private bool IsGround = false;
    private float ver;


    
    private Animator Anim;
    private Rigidbody2D Rbody;
    private SpriteRenderer Sprite;

    // Родитель игрока в сцене (объект Player): сюда возвращаемся, сходя с движущейся платформы.
    private Transform _homeParent;

    void Start()
    {
        _homeParent = transform.parent;
        Rbody = GetComponent<Rigidbody2D>();
        Sprite = GetComponentInChildren<SpriteRenderer>();
        Anim = GetComponent<Animator>();
    }


    float speed = 10.0f;

    void FixedUpdate()
    {
        Vector3 dir = Vector3.zero;

        dir.x = Vector3.Dot(Input.acceleration, Vector3.right) * Speed;
        
        if (dir.sqrMagnitude > 1)
            dir.Normalize();
        
        dir *= Time.deltaTime;
        
        transform.Translate(dir * speed);
    }

    
    
    public void ButtonRight()
    {
        Anim.SetBool("IsRun", true);
        Speed = horizontalspeed;
        Sprite.flipX = false;
    }

    public void ButtonLeft()
    {
        Anim.SetBool("IsRun", true);
        Speed = -horizontalspeed;
        Sprite.flipX = true;
    }

    public void Stop()
    {
        Anim.SetBool("IsRun", false);
        Anim.SetBool("IsJump", false);
       // Speed = 0;
    }

    public void JumpButton()
    {
        if (IsGround)
        {
            Anim.SetBool("IsJump", true);
            Rbody.AddForce(transform.up * JumpForce, ForceMode2D.Impulse);
        }
    }

    public void Run()
    {
        //transform.Translate(Speed * Time.deltaTime, 0, 0);
        //Клавиатурное управление
       
    }

    public void Jump()
    {
        Rbody.AddForce(transform.up * JumpForce, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Ground")))
        {
            Anim.SetBool("IsJump", false);
            IsGround = true;
        }
        if (collision.gameObject.CompareTag("PlatformMove"))
        {
            Anim.SetBool("IsJump", false);
            IsGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Ground")))
        {
            IsGround = false;
        }
        if (collision.gameObject.CompareTag("PlatformMove"))
        {
            IsGround = false;
            DetachFromPlatform(collision.transform);
        }
    }  

    // Пока платформа выключается (например, при перезагрузке сцены), Unity запрещает менять родителя —
    // в этот момент ничего не делаем. SetParent вызывается только при реальной смене платформы.
    private void AttachToPlatform(Transform platform)
    {
        if (transform.parent != platform && platform.gameObject.activeInHierarchy && gameObject.activeInHierarchy)
        {
            transform.SetParent(platform);
        }
    }

    private void DetachFromPlatform(Transform platform)
    {
        if (transform.parent == platform && platform.gameObject.activeInHierarchy && gameObject.activeInHierarchy)
        {
            transform.SetParent(_homeParent);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGround = true;
        }
        if (collision.gameObject.CompareTag("PlatformMove"))
        {
            IsGround = true;
            AttachToPlatform(collision.transform);
        }
    }

    public PlayerController Заміняє
    {
        get => default;
        set
        {
        }
    }
}
