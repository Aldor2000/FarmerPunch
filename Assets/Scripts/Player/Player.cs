using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    
    //Animation Variables
    [SerializeField] Animator anim;
    private Vector2 lastMoveDirection = new Vector2(0,-1);
    private bool facingLeft = false;

    //movement variables
    [SerializeField] float moveSpeed = 1.0f;
    private Vector2 movement;
    [SerializeField] Transform flipTarget;

    //Health support
    public int maxHealth = 3;
    public int currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //get components
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        //process inputs
        ProcessInputs();
        //animate
        Animate();
        //flip
        if((movement.x < 0 && !facingLeft) || (movement.x > 0 && facingLeft))
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        //movement with physics
        if (movement == Vector2.zero)
            rb.linearVelocity = Vector2.zero;
        else
            rb.linearVelocity = movement * moveSpeed;
    }


    void ProcessInputs()
    {

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement.Normalize();

        // Save the last non-zero direction
        if (movement != Vector2.zero)
        {
            lastMoveDirection = movement;
        }
    }

    void Animate()
    {
        anim.SetFloat("MoveX", movement.x);
        anim.SetFloat("MoveY", movement.y);
        anim.SetFloat("MoveMagnitude", movement.magnitude);
        anim.SetFloat("LastMoveX", lastMoveDirection.x);
        anim.SetFloat("LastMoveY", lastMoveDirection.y);

    }

    void Flip()
    {
        Vector3 scale = flipTarget.localScale;
        scale.x *= -1; // makes negative
        flipTarget.localScale = scale;
        facingLeft = !facingLeft;
    }

    public void TakeDamage(float damage)
    {

    }
    
}
