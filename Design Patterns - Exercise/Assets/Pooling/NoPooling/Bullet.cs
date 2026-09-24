using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoPooling
{
    public class Bullet : MonoBehaviour
    {
        Renderer m_Renderer;
        // Use this for initialization
        void Start()
        {
            m_Renderer = GetComponent<Renderer>();
        }

        // Update is called once per frame
        void Update()
        {
            if (m_Renderer.isVisible)
            {
                Debug.Log("Object is visible");
            }
            else
            {
                Debug.Log("Object is no longer visible");
                Destroy(gameObject);
            }
        }
    }
}
