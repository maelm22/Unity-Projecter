using UnityEngine;

public class Mario : Agent
{
    public int score;

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

    protected override void FiniteStateMachine()
    {
        switch (state)
        {
            case 0:
                //get coins
                MoveTo(GetClosestCoin());

                //if boo is close, then state = 1 
                if (Vector3.Distance(transform.position, GetClosestBoo().position) < 3)
                    state = 1;
                break;
            case 1:
                //escape
                print("I'm scared D:");
                break;
        }
    }

    private Transform GetClosestCoin()
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

        return closestCoin.transform;
    }

    private Transform GetClosestBoo()
    {
        var coins = GameObject.FindGameObjectsWithTag("Boo");
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

        return closestCoin.transform;
    }
}