using UnityEngine;

public class jump : MonoBehaviour
{
    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space)) Jump();
    }

    public void Jump()
    {
        rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
    }
}