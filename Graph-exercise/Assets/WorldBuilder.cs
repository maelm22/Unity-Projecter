using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class builds the world (just using Gizmos) for now, based on a graph
/// </summary>
public class WorldBuilder : MonoBehaviour
{
    public ZorkGraph map;

    // Here you can try to build your map
    void Start()
    {
        map = new ZorkGraph();
        //example of creating an area
        ZorkArea a1 = new ZorkArea("start");
        ZorkArea a2 = new ZorkArea("end");
        map.nodes.Add(a1);
        map.nodes.Add(a2);
        //example of establishing a connection
        map.AddConnection(a1, a2, Direction.North);


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        //uncomment this line if you want to force remake the graph (it will make a new one at each re-draw)
        //map = null;

        if (map == null)
            Start();

        map.DrawGraphWithGizmos();
    }
}
