using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boo : Agent {

    private Transform mario;
    public float minimumDistance = 6f;
    public float minimumAngle = 80f;
    private bool canMove;

    private void Start()
    {
        mario = GameObject.Find("Mario").transform;
        canMove = true;
    }

    protected override void FiniteStateMachine()
    {
        if (!CanMarioSeeMe() && canMove)
        {
          MoveTo(mario);  
        }
        
        //FSM ideas: chase mario, if mario sees the boo, freeze for some time and then return to chase
    }

    bool CanMarioSeeMe()
    {
        //Debug.Log(Vector3.Distance(transform.position, mario.position));
        //Debug.Log(Vector3.Angle(transform.position, mario.forward));
        if (Vector3.Distance(transform.position, mario.position) < minimumDistance &&
            Vector3.Angle(transform.position, mario.forward) < minimumAngle)
        {
            Debug.Log("He sees me!");
            
            
            StartCoroutine(WaitForAMoment());
            return true;
        }
        else
            return false;
    }

    private IEnumerator WaitForAMoment()
    {
        nm.isStopped = true;
        canMove = false;
        yield return new WaitForSeconds(2);
        nm.isStopped = false;
        canMove = true;
    }
    
}
