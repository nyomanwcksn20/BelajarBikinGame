using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // SerializeField digunakan untuk mengubah langsung "speed" di unity
    [SerializeField] private float speed;
    [SerializeField] private float jump;
    private Rigidbody2D rb;
    private Animator anim;
    private bool grounded;

    private void Awake()
    {
        // GetComponent untuk mengakses RigidBody2D dan Animator
        rb = GetComponent<Rigidbody2D>(); 
        anim = GetComponent<Animator>();

    }

    private void Update()
    {
        float horizonalInput = Input.GetAxis("Horizontal");

        // rb.velocity untuk memindahkan karakter
        rb.velocity = new Vector2(horizonalInput * speed, rb.velocity.y);

        //putar badan player saat kekiri
        if (horizonalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizonalInput < -0.01f)
            transform.localScale = new Vector3(-1,1,1);

        // lompat
        if (Input.GetKey(KeyCode.Space) && grounded)
            Jump();

        //mengatur parameter animasi (dimana jika arrow keys tidak ditekan maka horizontal input = 0)
        anim.SetBool("Walk", horizonalInput != 0);
        anim.SetBool("Grounded", grounded);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jump);
        anim.SetTrigger("Jump");
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            grounded = true;
    }
}
