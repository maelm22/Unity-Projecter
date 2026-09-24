using System.Collections.Generic;
using UnityEngine;

public class Swarm : MonoBehaviour
{
    public static List<GameObject> boids = new();
    public int swarmSize = 100;
    public GameObject boidPrefab;

    private bool parallel = false;

    // Start is called before the first frame update
    private void Start()
    {
        for (var i = 0; i < swarmSize; i++)
        {
            var position = Random.insideUnitCircle * 20;
            var boid = Instantiate(boidPrefab, position, Quaternion.identity);
            boids.Add(boid);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            foreach (var boid in boids)
            {
                var toBoid = boid.transform.position - mousePosition;
                boid.GetComponent<Rigidbody2D>().linearVelocity = toBoid / toBoid.magnitude * 50;
            }
        }
    }

    public void SpawnBoids()
    {
        var position = Random.insideUnitCircle * 20;
        var boid = Instantiate(boidPrefab, position, Quaternion.identity);
        boids.Add(boid);
    }
}