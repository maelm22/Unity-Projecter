using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoidJob : MonoBehaviour
{
    [Range(10f, 50f)] public float maxVelocity = 50f;

    [Range(1f, 50f)] public float mouseAttraction = 10f;

    public float range = 5;
    public float angleOfView = 160;
    public Color slow = Color.green;
    public Color fast = Color.cyan;

    private readonly List<Vector3> boidpos = new();
    private Rigidbody2D rb;

    private SpriteRenderer sr;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Random.insideUnitCircle * 10;
        sr = GetComponent<SpriteRenderer>();
    }


    // Update is called once per frame
    private void Update()
    {
        listConvert();
        //calculate the velocity vectors for all different criterias
        var s = Separation();
        var a = Alignment();
        var c = Cohesion();
        var o = ObstacleAvoidance();
        var t = FollowMouse();

        rb.linearVelocity += s /*+ a + c*/ + t;
        //if you want you can also limit the velocity to a maximum value (remember to change the value in the prefab)
        LimitVelocity();

        //this bit of code rotates the sprite so it points to the direction of the velocity
        var v = rb.linearVelocity;
        var angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //small color lerp to show how fast the boid is moving :)
        sr.color = Color.LerpUnclamped(slow, fast, rb.linearVelocity.magnitude / maxVelocity);
    }

    public Rigidbody2D GetRigidbody2D()
    {
        return rb;
    }

    private void listConvert()
    {
        boidpos.Clear();
        foreach (var boid in Swarm.boids) boidpos.Add(boid.transform.position);
    }

    private Vector2 Separation()
    {
        //how do we find the visible boids?
        // CanSee(GameObject boid) method that tells us if a boid is visible or not

        var vector = Vector2.zero;
        var pos = transform.position;
        var vel = rb.linearVelocity;

        Parallel.ForEach(boidpos, boid =>
        {
            if (CanSee(pos, boid, vel) && boid != pos) vector += (Vector2)(boid - pos); // Weight by distance
        });


        return vector * -1;
    }

    private bool CanSee(Vector3 boid, Vector3 target, Vector2 velocity)
    {
        if (Vector3.Distance(boid, target) < range &&
            Vector3.Angle(velocity, target - boid) < angleOfView)
            return true;
        return false;
    }

    private Vector2 Alignment()
    {
        var vector = rb.linearVelocity;
        var numberBoids = 0;

        foreach (var boid in Swarm.boids)
            if (CanSee(boid) && boid != gameObject)
            {
                numberBoids++;
                vector += boid.GetComponent<Rigidbody2D>().linearVelocity; // Weight by distance
            }

        if (numberBoids > 0) return vector / numberBoids;

        return Vector2.zero;
    }

    private Vector2 Cohesion()
    {
        var vector = Vector2.zero;
        var numberBoids = 0;


        foreach (var boid in Swarm.boids)
            if (CanSee(boid) && boid != gameObject)
            {
                numberBoids++;
                vector += (Vector2)boid.transform.position; // Weight by distance
            }

        if (numberBoids > 0) return vector / numberBoids - (Vector2)transform.position;

        return Vector2.zero;
    }

    //makes the boid move away from obstacles
    private Vector2 ObstacleAvoidance()
    {
        var vector = transform.position;
        var numberBoids = 0;

        var colliders = Physics2D.OverlapCircleAll(transform.position, range);

        foreach (var collider in colliders)
            if (collider.CompareTag("Obstacle"))
                if (CanSee(collider.gameObject))
                {
                    numberBoids++;
                    vector += collider.transform.position;
                }

        return (vector * -1).normalized;
    }

    //makes the boid attracted to the current mouse position by calculating the direction vector and "pushing" the boid by mouseAttraction
    private Vector2 FollowMouse()
    {
        var target = Vector3.zero;
        //Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return (target - transform.position).normalized * mouseAttraction * Time.deltaTime;
    }

    //clamps the velocity to the value specified in maxVelocity
    private void LimitVelocity()
    {
        if (rb.linearVelocity.magnitude > maxVelocity)
            rb.linearVelocity = rb.linearVelocity / rb.linearVelocity.magnitude * maxVelocity;
    }

    //returns true if the boid in input can be seen by this boid, else false
    private bool CanSee(GameObject boid)
    {
        if (Vector3.Distance(transform.position, boid.transform.position) < range &&
            Vector3.Angle(rb.linearVelocity, boid.transform.position - transform.position) < angleOfView)
            return true;
        return false;
    }
}