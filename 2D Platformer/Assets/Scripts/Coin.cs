using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Coin : MonoBehaviour
{
    //private AudioClip audioSource;

    
    public AudioClip coinSound;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

   private void OnTriggerEnter2D(Collider2D collision)
   {
       if (collision.gameObject.CompareTag("Player"))
       {
            
            collision.gameObject.GetComponent<AudioSource>().PlayOneShot(coinSound);
            Destroy(gameObject);
            //MeshCollider.enabled = false;
        }
   
   }

   
   // private void OnTriggerEnter2D(Collider2D collision)
   // {
   //     if (collision.gameObject.CompareTag("Player"))
   //     {
   //         Destroy(gameObject);
   //     }
   // }

    

}
