using UnityEngine;

public class AgentExample : TreeHandler
{
    [Header("Action Variables")] [SerializeField]
    private Transform pointA;

    [SerializeField] private Transform pointB;
    [SerializeField] private Transform pointC;
    private float counter;
    private bool isRunning;
    private float walkCounter;

    private void Start()
    {
        InitTree();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) isRunning = !isRunning;

        if (isRunning) Execute();
    }

    public void Count()
    {
        counter += Time.deltaTime;
        if (counter > 3)
        {
            Debug.Log("3s has passed");
            counter = 0;
            Callback(true);
        }
    }

    public void WalkToA()
    {
        if (transform.position != pointA.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, pointA.position, Time.deltaTime);
        }
        else
        {
            Debug.Log("Reached point A");
            Callback(true);
        }
    }

    public void WalkToB()
    {
        if (transform.position != pointB.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, pointB.position, Time.deltaTime);
        }
        else
        {
            Debug.Log("Reached point B");
            Callback(true);
        }
    }

    public void WalkToC()
    {
        walkCounter += Time.deltaTime;
        if (walkCounter > 5)
        {
            walkCounter = 0;
            Debug.Log("Time's up. Didn't reach point C");
            Callback(false);
            return;
        }

        if (transform.position != pointC.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, pointC.position, Time.deltaTime);
        }
        else
        {
            Debug.Log("Reached point C");
            Callback(true);
        }
    }
}