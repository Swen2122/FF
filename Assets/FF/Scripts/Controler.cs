using UnityEngine;

public class Controler : MonoBehaviour
{
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer SRenderer;

    public static Controler Instance { get; private set; }
    public bool CanDash { get; set; } = true;
    // Movement
    float horizontal;
    float vertical;
    private float lastMoveDirection = 1f;
    [SerializeField] private float moveLimiter = 0.7f;
    public float runSpeed = 5.0f;
    [SerializeField] private float acceleration = 50f; // Прискорення
    [SerializeField] private float deceleration = 30f; // Сповільнення

    // Dash
    [SerializeField] private float dash_distance = 3f;
    [SerializeField] private float dash_duration = 0.2f;
    private Vector2 dash_cord;
    private Vector3 lastDirection = Vector3.zero;
    public LayerMask obstacleLayer;

    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this; // Зберігаємо посилання на поточний об'єкт
        }
    else
        {
            Destroy(gameObject); // Видаляємо зайві копії, якщо вони є
        }
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        SRenderer = GetComponent<SpriteRenderer>();

        // Налаштування фізики
        body.linearDamping = 1f; // Додаємо опір руху
        body.constraints = RigidbodyConstraints2D.FreezeRotation; // Запобігаємо обертанню
    }

    void Update()
    {
        if(PauseManager.IsPaused) return;
        HandleInput();
        UpdateAnimation();
    }

    void HandleInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (Input.GetMouseButtonDown(1))
        {
            StartAnim("fire2");
        }

        if (horizontal != 0 && horizontal != lastMoveDirection)
        {
            Flip();
        }

        if (Input.GetButtonDown("Shift"))
        {
            Dash();
        }
    }

    void UpdateAnimation()
    {
        float speed = Mathf.Abs(body.linearVelocity.magnitude);
        anim.SetFloat("speed", speed);
    }

    void FixedUpdate()
    {
        if(PauseManager.IsPaused) return;
        Move();
    }

    void Move()
    {
        // Отримуємо бажаний напрямок руху
        Vector2 moveDirection = new Vector2(horizontal, vertical);

        // Нормалізуємо діагональний рух
        if (moveDirection.magnitude > 1f)
        {
            moveDirection.Normalize();
        }

        // Застосовуємо діагональний ліміт
        if (horizontal != 0 && vertical != 0)
        {
            moveDirection *= moveLimiter;
        }

        // Розраховуємо цільову швидкість
        Vector2 targetVelocity = moveDirection * runSpeed;

        // Поточна швидкість
        Vector2 currentVelocity = body.linearVelocity;

        // Плавно змінюємо швидкість до цільової
        float accelRate = (moveDirection.magnitude > 0.1f) ? acceleration : deceleration;

        // Використовуємо Vector2.Lerp для плавного переходу
        Vector2 newVelocity = Vector2.MoveTowards(
            currentVelocity,
            targetVelocity,
            accelRate * Time.fixedDeltaTime
        );

        // Застосовуємо нову швидкість через AddForce
        body.linearVelocity = newVelocity;
    }

    void Dash()
    {
        if(!CanDash) return;
        Vector2 dashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        if (dashDirection.magnitude < 0.1f)
        {
            dashDirection = SRenderer.flipX ? Vector2.left : Vector2.right;
        }
        PlayerTracker.Instance.SetDash();
        DashUtility.PerformDash(
            body,
            dashDirection,
            dash_distance,
            dash_duration,
            obstacleLayer
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
    public Vector3 GetPlayerPosition()
    {
        return transform.position;
    }
        public Transform GetPlayerTranform()
    {
        return transform;
    }
}