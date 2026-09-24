using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoPooling
{
    public class ShootBulletOnClick : MonoBehaviour
    {
        public GameObject bulletPrefab;
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

                // Instantiate the bullet prefab and set its position and rotation
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

                // Set the bullet's velocity based on the direction and bullet speed
                Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
                bulletRigidbody.velocity = direction.normalized * bulletSpeed;
            }
        }
    }
}
