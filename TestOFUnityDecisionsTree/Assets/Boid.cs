using System.Collections;
using System.Linq;
using UnityEngine;

public class Boid : MonoBehaviour
{
    private static readonly Color SlowColor = Color.green;
    private static readonly Color FastColor = Color.cyan;
    [SerializeField] private GameObject boidPrefab;
    [SerializeField] [Range(10f, 50f)] private float maxVelocity = 50f;
    [SerializeField] private float minVelocity = 10f;
    [SerializeField] [Range(1f, 50f)] private float mouseAttraction = 10f;
    [SerializeField] private float range = 5f;
    [SerializeField] private float angleOfView = 160f;
    private GameObject[] obstacles;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private int timeToDeath = 10;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Random.insideUnitCircle * 10;
        sr = GetComponent<SpriteRenderer>();
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        StartCoroutine(DeathCoroutine());
    }

    private void Update()
    {
        var velocityChange = Separation() + Alignment() + Cohesion() + ObstacleAvoidance() + FollowMouse();
        rb.linearVelocity += velocityChange;
        LimitVelocity();
        RotateBoid();
        UpdateColor();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            Debug.Log("Got a coin!");
            Destroy(other.gameObject);
            var position = (Vector2)transform.position + Random.insideUnitCircle * 5;
            var boid = Instantiate(boidPrefab, position, Quaternion.identity);
            Swarm.boids.Add(boid);
            mouseAttraction -= 10;
            mouseAttraction = Mathf.Clamp(mouseAttraction, 1, 50);
            timeToDeath += 5;
        }
    }

    private IEnumerator DeathCoroutine()
    {
        while (timeToDeath > 0)
        {
            yield return new WaitForSeconds(1);
            timeToDeath--;
            mouseAttraction += 2;
            mouseAttraction = Mathf.Clamp(mouseAttraction, 1, 50);
            maxVelocity = -1;
            maxVelocity = Mathf.Clamp(maxVelocity, minVelocity, 50);
        }

        Swarm.boids.Remove(gameObject);
        Destroy(gameObject);
    }

    private void RotateBoid()
    {
        var angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void UpdateColor()
    {
        sr.color = Color.LerpUnclamped(SlowColor, FastColor, rb.linearVelocity.magnitude / maxVelocity);
    }

    private Vector2 Separation()
    {
        var separationForce = Swarm.boids
            .Where(boid => CanSee(boid) && boid != gameObject)
            .Aggregate(Vector2.zero,
                (current, boid) => current + (Vector2)(boid.transform.position - transform.position));

        return -separationForce;
    }

    private Vector2 Alignment()
    {
        var alignmentForce = rb.linearVelocity;
        var count = 0;

        foreach (var boid in Swarm.boids.Where(boid => CanSee(boid) && boid != gameObject))
        {
            count++;
            alignmentForce += boid.GetComponent<Rigidbody2D>().linearVelocity;
        }

        return count > 0 ? alignmentForce / count : Vector2.zero;
    }

    private Vector2 Cohesion()
    {
        var cohesionForce = Vector2.zero;
        var count = 0;

        foreach (var boid in Swarm.boids.Where(boid => CanSee(boid) && boid != gameObject))
        {
            count++;
            cohesionForce += (Vector2)boid.transform.position;
        }

        return count > 0 ? cohesionForce / count - (Vector2)transform.position : Vector2.zero;
    }

    private Vector2 ObstacleAvoidance()
    {
        var avoidanceForce = obstacles
            .Where(CanSee)
            .Aggregate(Vector2.zero,
                (current, obstacle) => current + (Vector2)(obstacle.transform.position - transform.position));

        return -avoidanceForce * 20;
    }

    private Vector2 FollowMouse()
    {
        var closestCoin = GetClosestCoin();
        if (closestCoin != null)
        {
            var target = closestCoin.position;
            return (target - transform.position) * (mouseAttraction * Time.deltaTime);
        }

        return Vector2.zero;
    }

    private Transform GetClosestCoin()
    {
        var coins = GameObject.FindGameObjectsWithTag("Coin");
        var minDistance = float.PositiveInfinity;
        GameObject closestCoin = null;

        foreach (var coin in coins)
            if (coin != null)
            {
                var distance = Vector3.Distance(coin.transform.position, transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestCoin = coin;
                }
            }

        return closestCoin?.transform;
    }

    private void LimitVelocity()
    {
        if (rb.linearVelocity.magnitude > maxVelocity) rb.linearVelocity = rb.linearVelocity.normalized * maxVelocity;
    }

    private bool CanSee(GameObject boid)
    {
        return Vector3.Distance(transform.position, boid.transform.position) < range &&
               Vector3.Angle(rb.linearVelocity, boid.transform.position - transform.position) < angleOfView;
    }
}