using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario : Agent {

    public int score = 0;
    public float booThreshold = 3;

    private Transform closestBoo;

    protected override void FiniteStateMachine()
    {
        if (IsBooClose())
        {
            transform.LookAt(closestBoo.position);
            MoveAway();
        }
        else if (!IsBooClose())
        {
            MoveTo(GetClosestCoin()); 
        }
        
        
        
        
        //FSM ideas: mario goes for the closest coin, if a boo is close he runs away (or maybe turns to look at the boo), if he's far enough then he goes back to look for coins
    }

    Transform GetClosestCoin()
    {
        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
        float minDistance = float.PositiveInfinity;
        GameObject closestCoin = null;
        foreach (GameObject coin in coins)
        {
            float distance = Vector3.Distance(coin.transform.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestCoin = coin;
            }
        }
        return closestCoin.transform;
    }

    bool IsBooClose()
    {
        GameObject[] boos = GameObject.FindGameObjectsWithTag("Boo");
        float minDistance = float.PositiveInfinity;
        foreach (GameObject boo in boos)
        {
            float distance = Vector3.Distance(boo.transform.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestBoo = boo.transform;
            }
        }
        if (minDistance < booThreshold)
            return true;
        else
            return false;
    }

    private void MoveAway()
    {
        nm.SetDestination(transform.position+(closestBoo.position.normalized * -1));
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
}
