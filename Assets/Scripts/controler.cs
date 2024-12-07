using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controler : MonoBehaviour
{
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer SRenderer;
    //run
    float horizontal;
    float vertical;
    private float lastMoveDirection = 1f;
    [SerializeField] private float moveLimiter = 0.7f;
    [SerializeField] private float runSpeed = 5.0f;
    //dash
    [SerializeField] private float dash_distance = 3f;
    [SerializeField] private float dash_duration = 0.2f;
    private Vector2 dash_cord;
    private Vector3 lastDirection = Vector3.zero; //останній напрямок
    public LayerMask obstacleLayer; // шар з яким стикаєшся при деші

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        SRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            StartAnim("fire2");
        }
        if (horizontal != 0 && horizontal != lastMoveDirection)
        {
            Flip();
        }
        horizontal = Input.GetAxisRaw("Horizontal"); // 1/-1 right/left
        vertical = Input.GetAxisRaw("Vertical"); // 1/-1 up/down
        float speed = Mathf.Abs(horizontal + vertical);
        anim.SetFloat("speed", speed);

        // Виклик дашу через кнопку Shift
        if (Input.GetButtonDown("Shift"))
        {
            Dash();
        }
    }

    void FixedUpdate()
    {
        if (horizontal != 0 && vertical != 0) // перевірка на діагональний рух
        {
            // зменшення швидкості при діагональному русі
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
        }
        body.linearVelocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
    }

    void Dash()
    {
        Vector2 lastDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        lastDirection.Normalize();
        // Викликаємо статичну функцію DashUtility для виконання дашу
        DashUtility.PerformDash(
            body,                  // Передаємо Rigidbody2D персонажа
            lastDirection,         // Напрямок дашу
            dash_distance,         // Дистанція дашу
            dash_duration,         // Тривалість дашу
            obstacleLayer          // Шари перешкод
        );
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