using UnityEngine;
using UnityEngine.AI;

public class MarioTree : TreeHandler
{
    public int score;


    public NavMeshAgent nm;
    public Transform target;
    private Transform closestBoo;
    private Transform closestCoin;

    private Vector3 runTarget;

    // Start is called before the first frame update
    private void Start()
    {
        InitTree();
    }

    // Update is called once per frame
    private void Update()
    {
        Execute();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Coin")
        {
            score++;
            Debug.Log("Got a coin!");
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Boo")
        {
            Debug.Log("Mamma mia! [score:" + score + "]");
            Destroy(gameObject);
        }
    }

    public void GetCoin()
    {
        if (closestCoin == null)
        {
            Callback(false);
            return;
        }

        MoveTo(closestCoin);
        if (Vector3.Distance(transform.position, closestCoin.position) < 0.5f)
            Callback(true);
    }

    public void MoveTo(Transform destination)
    {
        nm.SetDestination(destination.position);
    }

    public void GetRunTarget()
    {
        var direction = transform.position - closestBoo.position;
        //direction = Vector3.Normalize(direction);
        runTarget = direction;
        Callback(true);
    }

    public void Run()
    {
        if (closestCoin == null)
        {
            Callback(false);
            return;
        }

        nm.SetDestination(runTarget);
        if (Vector3.Distance(transform.position, closestCoin.position) < 0.5f)
            Callback(true);
    }

    public void IsBooClose()
    {
        if (Vector3.Distance(transform.position, closestBoo.position) < 3)
            Callback(true);
        else
            Callback(false);
    }

    public void GetClosestCoin()
    {
        var coins = GameObject.FindGameObjectsWithTag("Coin");
        var minDistance = float.PositiveInfinity;
        GameObject closestCoin = null;
        foreach (var coin in coins)
        {
            var distance = Vector3.Distance(coin.transform.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestCoin = coin;
            }
        }

        if (closestCoin == null)
        {
            Callback(false);
        }
        else
        {
            this.closestCoin = closestCoin.transform;
            Callback(true);
        }
    }

    public void GetClosestBoo()
    {
        var coins = GameObject.FindGameObjectsWithTag("Boo");
        var minDistance = float.PositiveInfinity;
        GameObject closestBoo = null;
        foreach (var coin in coins)
        {
            var distance = Vector3.Distance(coin.transform.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestBoo = coin;
            }
        }

        if (closestBoo == null)
        {
            Callback(false);
        }
        else
        {
            this.closestBoo = closestBoo.transform;
            Callback(true);
        }
    }
}