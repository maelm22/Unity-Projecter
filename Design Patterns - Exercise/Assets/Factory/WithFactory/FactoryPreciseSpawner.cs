using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryPreciseSpawner : MonoBehaviour
{
    public Powerup spawnType = Powerup.Cube;
    // Start is called before the first frame update
    void Start()
    {
        GameObject go = PowerupFactory.Create(spawnType);

        //these lines change position of the object and give it some movement, just for fun :) 
        go.transform.position = transform.position;
        Rigidbody rb = go.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
        rb.AddTorque(Random.onUnitSphere * 20,ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
