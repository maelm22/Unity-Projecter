using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreciseSpawner : MonoBehaviour
{
    public GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {
        GameObject go = Instantiate(prefab, transform.position,Quaternion.identity);

        //these lines change position of the object and give it some movement, just for fun :) 
        Rigidbody rb = go.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
        rb.AddTorque(Random.onUnitSphere * 20, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
