using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PLayerController : MonoBehaviour
{
    Rigidbody2D rb;

    public Text coinAmount;
    public int coinsCollected = 0;
    public int coinsTotal = 16;

    public float speed = 10;
    public float maxSpeed = 10;
    public float jumpForce = 10;
    public float gravityScale = 1;
    public float fallingGravityScale = 5;

    SpriteRenderer sr;

    private AudioSource audioSource;


    bool jump = false;
    bool onGround;
   //bool goingLeft;
   //bool goingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        audioSource = GetComponent<AudioSource>();
        

        audioSource.time = 2.3f;

        sr = GetComponent<SpriteRenderer>();

        coinAmount.text = "Coins Collected: " + coinsCollected;
    }

    // Update is called once per frame
    void Update()
    {
      // if (Input.GetKeyDown(KeyCode.LeftArrow) && goingRight)
      // {
      //     // left
      //     goingRight = false;
      //     transform.Rotate(0f, 180f, 0f) ; 
      //     goingLeft = true;
      // }
      //
      // if (Input.GetKeyDown(KeyCode.RightArrow) && goingLeft)
      // {
      //     // right
      //     goingLeft = false;
      //     transform.Rotate(0f, 180f, 0.0f);  
      //     goingRight = true;  
      // }

        

        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
     
            jump = true;

            audioSource.Play();

        }
        if (rb.velocity.y >= 0)
        {
            rb.gravityScale = gravityScale;
        }
        else if (rb.velocity.y < 0)
        {
            rb.gravityScale = fallingGravityScale;
        }

        coinAmount.text = "Coins Collected: " + coinsCollected;
    }


    

    private void FixedUpdate()
    {
        float direction = Input.GetAxis("Horizontal"); // [-1,1] left/right
        rb.AddForce(Vector2.right * direction * speed);

        if (rb.velocity.x > maxSpeed)
        {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
        }
        if (rb.velocity.x < -maxSpeed)
        {
            rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);
        }

        if (jump == true)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jump = false;

        }
        if (direction > 0)
        {
            sr.flipX = false;
        }
        if (direction < 0)
        {
            sr.flipX = true;
        }

        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DeathTrigger"))
        {
            SceneManager.LoadScene("MainScene");
        }
       if (collision.gameObject.CompareTag("Coin"))
       {
            
            coinsCollected++;
       }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            onGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            onGround = false;
        }
    }
}
