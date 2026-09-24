using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    
    

    // Start is called before the first frame update
    void Start()
    {
        float x = Random.Range(-9, 9);
        float y = Random.Range(0.5f, 2);
        float z = Random.Range(-9, 9);   
        transform.Translate(x, y, z);
        int colorIndex = Random.Range(0, 100);
        gameObject.GetComponent<MeshRenderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        float movement = Random.Range(0, 50);
        transform.Rotate(new Vector3(movement, movement, movement) * Time.deltaTime);
    }
}
