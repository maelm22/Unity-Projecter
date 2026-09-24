using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    Rigidbody2D rb;

    
    public float movementSpeed = 10;
    public bool moveRight;
    public bool moveLeft = true;

    public Vector3 startPosition;
    public Vector3 endPosition;
    Vector3 currentPosition;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPosition = endPosition;

        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(0f, 180f, 0f);
        
        transform.position = Vector3.MoveTowards(transform.position, currentPosition, movementSpeed * Time.deltaTime);
        
        if (transform.position == endPosition)
        {
            currentPosition = startPosition;
        }
        if (transform.position == startPosition)
        {
            currentPosition = endPosition;
        }




    }
}
