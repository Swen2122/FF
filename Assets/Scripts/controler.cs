using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controler : MonoBehaviour
{
    [SerializeReference] private Rigidbody2D body;
    [SerializeReference] private Animator anim;
    [SerializeReference] private SpriteRenderer SRenderer;
    //run
    float horizontal;
    float vertical;
    private float lastMoveDirection = 1f;
    [SerializeReference] private float moveLimiter = 0.7f;
    [SerializeReference] private float runSpeed = 5.0f;
    //dash
    [SerializeReference] private float dash_force = 20f;
    public float dash_duration = 0.2f;
    private Vector2 dash_cord;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        SRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Shift"))
        {
            StartAnim("dash");
            Dash();
        }
        if (Input.GetMouseButtonDown(1))
        {
            StartAnim("fire2");
        }
        if (horizontal != 0 && horizontal != lastMoveDirection)
        {
            Flip();
        }



        // Отримує напрямок руху
        horizontal = Input.GetAxisRaw("Horizontal"); // 1/-1 право/ліво
        vertical = Input.GetAxisRaw("Vertical"); // 1/-1 верх/вниз
        float speed = Mathf.Abs(horizontal + vertical);
        anim.SetFloat("speed", speed);
    }
   

    void FixedUpdate()
    {
        if (horizontal != 0 && vertical != 0) // перевірка діагонального руху
            {
                // зменшення швидкості на 70% при діагональному русі
                horizontal *= moveLimiter;
                vertical *= moveLimiter;
            }
        body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed)+ dash_cord;
        if (dash_cord.magnitude > 0)
        {
            dash_cord *= 0.9f; // Наприклад, на 10% за кадр
        }
    }

      void Dash()
       {
        /* dash_cord = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
         dash_cord.Normalize(); */
        dash_cord = new Vector2(horizontal, vertical);
        dash_cord *= dash_force;        
       }
    
    void StartAnim(string anim_name)
    {
        anim.SetTrigger(anim_name);
    }

    void Flip()
    {
        SRenderer.flipX = !SRenderer.flipX;
        lastMoveDirection = horizontal;
    }

}

