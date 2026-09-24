using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


/// <summary>
/// class that represents a general graph. In this case the graph assumes that the information about the edges is included within the nodes themselves. You could also add a new list of edges to the graph to make it more explicit.
/// Pros of this implementation: you can start from any node and reconstruct a (potentially partial) picture of the graph. The edges information important to nodes is embedded in the nodes themselves
/// Cons: there isn't a centralized list of edges
/// Personally I think this implementation is nice when you expect your graph to change often and you want it to be flexible, if the graph is going to be mostly static, other implementations could be better
/// </summary>
public class Graph<TNodeType>
{
    public List<TNodeType> nodes { get; private set; } = new List<TNodeType>();
}

/// <summary>
/// This is a somewhat special implementation of a node, given that we give a label/key to each possible child (through the use of a dictionary). This means that we can limit the amount of neighbors each node can have up to some amount, depending on what TKeyType value is chosen, and that there can never be more than on neighbor with the same "label"
/// </summary>
public class Node<TKeyType, TValueType>
{
    //the dictionary that contains the information about neighbors
    public Dictionary<TKeyType, TValueType> neighbors = new Dictionary<TKeyType, TValueType>();
}

/// <summary>
/// The class containing the graph for Zork, it has a few specialized methods that are specific to the game (like dealing with Directions)
/// </summary>
public class ZorkGraph : Graph<ZorkArea>
{
    
    
    //Establishes a connection between two areas, based on a direction. 
    //Since Gizmos have to be drawn at some position in the Unity space, you should also updates the position field of "to" based on the connection (so that if the connection is from->to West, the "to" node ends up being to the left of "from")
    public void AddConnection(ZorkArea from, ZorkArea to, Direction direction)
    {
        //COMPLETE ME
        
        from.neighbors.Add(direction,to);
        Vector3 offset = new Vector3();
        switch (direction)
        {
            case Direction.North:
                offset = Vector3.up;
                break;
            case Direction.South:
                offset = Vector3.down;
                break;
            case Direction.East:
                offset = Vector3.right;
                break;
            case Direction.West:
                offset = Vector3.left;
                break;
        }

        to.position = from.position + offset;

    }

    //Use gizmos to draw all the nodes and all the connections
    //Hint: you already know how to find all areas, they're in "nodes", how can we get all the edges?
    //Hint: you can use DrawSphere and DrawLine to respectively draw nodes and edges
    //Extra: you could also use the gizmoColor field in ZorkArea to color the spheres :) 
    public void DrawGraphWithGizmos()
    {
        //COMPLETE ME 

        foreach (var vaNode in nodes)
        {
            Gizmos.DrawSphere(vaNode.position,(float)0.25);

            foreach (var node in vaNode.neighbors)
            {
                Gizmos.DrawLine(vaNode.position, node.Value.position);
            }
           
        }
    }
}

/// <summary>
/// The class that defines areas for Zork, this would contain all the pertaining info for your game area.
/// As we chose Direction as the label type, this means that each node can only have up to 4 neighbors
/// </summary>
public class ZorkArea : Node<Direction, ZorkArea>
{
    //various data
    public string name;
    public string description;
    public Vector3 position = Vector3.zero;
    public Color gizmoColor = UnityEngine.Random.ColorHSV();

    public ZorkArea(string name)
    {
        this.name = name;
    }
}

/// <summary>
/// Enum for the possible directions we can take from each area
/// </summary>
public enum Direction
{
    North,
    South,
    East,
    West
}
