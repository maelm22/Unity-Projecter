using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Text countText;
    public Text winText;
    public float jumpHeight;
    private Rigidbody rb;
    private int count;

    bool canJump = true;
    int maxJumps = 1;
    int currentJump = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winText.text = "";
    }

    
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);
        
        

         
       
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
         {
            rb.AddForce(Vector2.up * jumpHeight, ForceMode.Impulse);

            currentJump++;

            if (currentJump == maxJumps)
            {
                StartCoroutine(CoolDown());
            }

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
           
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 14)
        {
            winText.text = "You Win!";
        }
    }

    void DeathZone()
    {
        
    }

    private IEnumerator CoolDown()
    {
        currentJump = 0;
        canJump = false;
        yield return new WaitForSeconds(2);
        canJump = true;
    }
}
