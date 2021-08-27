using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // SerializeField digunakan untuk mengubah langsung "speed" di unity
    [SerializeField] private float speed;
    [SerializeField] private float jump;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D rb;
    private Animator anim;
    private CapsuleCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizonalInput;

    private void Awake()
    {
        // GetComponent untuk mengakses RigidBody2D dan Animator
        rb = GetComponent<Rigidbody2D>(); 
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        horizonalInput = Input.GetAxis("Horizontal");
        
        //putar badan player saat kekiri
        if (horizonalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizonalInput < -0.01f)
            transform.localScale = new Vector3(-1,1,1);

        //mengatur parameter animasi (dimana jika arrow keys tidak ditekan maka horizontal input = 0)
        anim.SetBool("Walk", horizonalInput != 0);
        anim.SetBool("Grounded", isGrounded());

        //logika wall jump
        if (wallJumpCooldown < 0.2f)
        {
           

            // rb.velocity untuk memindahkan karakter
            rb.velocity = new Vector2(horizonalInput * speed, rb.velocity.y);

            if (onWall() && !isGrounded())
            {
                rb.gravityScale = 0;
                rb.velocity = Vector2.zero;
            }
            else
                rb.gravityScale = 7;

            // lompat
            if (Input.GetKey(KeyCode.Space))
                Jump();
        }
        else
            wallJumpCooldown += Time.deltaTime;
    }

    private void Jump()
    {
        if (isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jump);
            anim.SetTrigger("Jump");
        }
        else if (onWall() && !isGrounded())
        {
            if(horizonalInput == 0)
            {
                rb.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 50, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
                rb.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 30, 16);
            wallJumpCooldown = 0;
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
     
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
}
