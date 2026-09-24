using UnityEngine;
using UnityEngine.AI;

public class BooTree : TreeHandler
{
    [SerializeField] private Transform mario;

    public float minimumDistance = 6f;
    public float minimumAngle = 80f;

    public NavMeshAgent nm;
    private Transform target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        InitTree();
    }

    // Update is called once per frame
    private void Update()
    {
        Execute();
    }

    public void Run()
    {
        if (target == null)
        {
            nm.isStopped = true;
            Callback(false);
            return;
        }

        nm.isStopped = false;
        nm.SetDestination(target.position);
        Callback(true);
    }


    public void CanMarioSeeMe()
    {
        //Debug.Log(Vector3.Distance(transform.position, mario.position));
        //Debug.Log(Vector3.Angle(transform.position, mario.forward));
        if (Vector3.Distance(transform.position, mario.position) < minimumDistance &&
            Vector3.Angle(transform.position, mario.forward) < minimumAngle)
        {
            Debug.Log("He sees me!");
            target = null;
        }
        else
        {
            target = mario;
        }

        Callback(true);
    }
}