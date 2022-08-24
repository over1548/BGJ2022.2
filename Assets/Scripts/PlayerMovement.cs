using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    private float moveInput;

    private Rigidbody2D rb;
    private bool facingRight = true;

    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    [SerializeField] Vector2 groundCheckSize;

    private int extraJumps;
    public int extraJumpsValue;

    private Animator anim;

    private bool canDash = true;
    private bool isDashing;
    public float dashingPower = 24f;
    public float dashingTime = 0.2f;
    public float dashingCoolDown = 1f;

    // Start is called before the first frame update
    void Start()
    {
        extraJumps = extraJumpsValue;
        rb = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(isDashing)
        {
            return;
        }

        if(isGrounded == true)
        {
            extraJumps = 1;
        }
//Jump
        if(Input.GetKeyDown(KeyCode.Space) && extraJumps > 0)
        {
            rb.velocity = Vector2.up * jumpForce;
            extraJumps--;
            anim.SetTrigger("Jump");
        }

        else if(Input.GetKeyDown(KeyCode.Space) && extraJumps == 0 && isGrounded == true)
        {
            rb.velocity = Vector2.up * jumpForce;
            anim.SetTrigger("Jump");
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            anim.SetTrigger("Dash");
            StartCoroutine(Dash());
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isDashing)
        {
            return;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position , checkRadius , whatIsGround);
//Move
        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2 (moveInput * speed , rb.velocity.y);
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            anim.SetBool("isRunning" , true);
        }
        else 
        {
            anim.SetBool("isRunning" , false);
        }

        if(facingRight == false && moveInput > 0)
        {
            Flip();
        }
        else if(facingRight == true && moveInput < 0)
        {
            Flip();
        }
    }
    
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower , 0f);
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCoolDown);
        canDash = true;
    }
}
