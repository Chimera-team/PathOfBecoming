
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Health))]
public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public Rigidbody2D fairyAnchor;
    public SpriteRenderer interactIndicator;
    public Transform firePoint;
    public Transform spellDirection;

    public Rigidbody2D rb { get; private set; }
    public bool isGround { get; private set; }
    public float checkRadius;
    public LayerMask whatIsGround;

    Health health;
    public ControlHandler buttonsControl { get; private set; }

    public Vector3 lastCheckpoint { get; private set; }

    private int extraJump;
    public int ExtraJumpValue;

    public void Initialise(Image healthBar)
    {
        health = GetComponent<Health>();
        health.Initialise(healthBar);
    }
    public void Load_State(PlayerData data)
    {
        health.Set_Health(data.hp);
        transform.position = data.lastCheckpoint.Convert_to_UnityVector();
        lastCheckpoint = transform.position;
    }

    public PlayerData Save_State()
    {
        lastCheckpoint = transform.position;
        return new PlayerData(SceneManager.GetActiveScene().buildIndex, new Vector3Serial(transform.position), health.Get_Health());
    }

    private void Awake()
    {
        isGround = Physics2D.OverlapCircle(transform.position, checkRadius, whatIsGround);
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        extraJump = ExtraJumpValue;
        lastCheckpoint = transform.position;
        Change_Controls<DefaultHandler>();
    }

    public void Jump()
    {
        if (isGround == true)
            extraJump = ExtraJumpValue;
        if (extraJump > 0)
        {
            rb.velocity = Vector2.up * jumpForce;
            extraJump--;
        }
        else if (extraJump == 0 && isGround == true)
        {
            rb.velocity = Vector2.up * jumpForce;
        }
    }

    private void Update()
    {
        buttonsControl.Action();
    }

    public void Move(int direction)
    {
        isGround = Physics2D.OverlapCircle(transform.position, checkRadius, whatIsGround);
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
    }

    public void Last_Checkpoint()
    {
        Engine.current.Last_Chekpoint(() => health.Heal(health.maxHealth));
    }

    public void Change_Controls<T>() where T : ControlHandler, new()
    {
        rb.velocity = Vector3.zero;
        buttonsControl = new T();
    }
}
