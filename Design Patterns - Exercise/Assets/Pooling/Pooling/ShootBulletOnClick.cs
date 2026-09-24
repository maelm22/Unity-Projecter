using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pooling
{
    public class ShootBulletOnClick : MonoBehaviour
    {
        public float bulletSpeed = 10f;

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Get the click position in world space
                Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                clickPosition.z = 0f;

                // Calculate the direction from the current position to the click position
                Vector3 direction = clickPosition - transform.position;

                // Get a bullet from the Pool and set its position
                GameObject bullet = Pool.GetObject();
                bullet.transform.position = transform.position;

                // Set the bullet's velocity based on the direction and bullet speed
                Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
                bulletRigidbody.velocity = direction.normalized * bulletSpeed;
            }
        }
    }
}
