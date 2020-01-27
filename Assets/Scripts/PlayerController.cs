using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed;
    public float JumpForce;
    private bool IsGround = false;
    private float ver;

    private Animator Anim;
    private Rigidbody2D Rbody;
    private SpriteRenderer Sprite;

    void Start()
    {
        Rbody = GetComponent<Rigidbody2D>();
        Sprite = GetComponentInChildren<SpriteRenderer>();
        Anim = GetComponent<Animator>();
    }

    void Update()
    {
        ver = Input.GetAxis("Vertical");
        if (ver == 0f)
            Anim.SetBool("IsRun", false);
        if (Input.GetButton("Horizontal"))
        {
            Anim.SetBool("IsRun", true);
            Run();
        }
        if (IsGround && Input.GetButtonDown("Jump"))
        {
            Anim.SetBool("IsJump", true);
            Jump();
        }
    }

    private void Run()
    {
        Vector3 direction = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, Speed * Time.deltaTime);

        Sprite.flipX = direction.x < 0f;
    }

    private void Jump()
    {
        Rbody.AddForce(transform.up * JumpForce, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.tag == "Ground"))
        {
            Anim.SetBool("IsJump", false);
            IsGround = true;
        }
        if (collision.gameObject.tag == "PlatformMove")
        {
            Anim.SetBool("IsJump", false);
            IsGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if ((collision.gameObject.tag == "Ground"))
        {
            IsGround = false;
        }
        if (collision.gameObject.tag == "PlatformMove")
        {
            IsGround = false;
            transform.SetParent(null);
        }
    }  

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            IsGround = true;
        }
        if (collision.gameObject.tag == "PlatformMove")
        {
            IsGround = true;
            transform.SetParent(collision.transform);
        }
    }
}
