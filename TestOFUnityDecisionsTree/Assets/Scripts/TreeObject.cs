using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tree", menuName = "ScriptableObjects/TreeObject", order = 1)]
public class TreeObject : ScriptableObject
{
    public List<Node> nodes = new();
    public List<NodeConnection> nodeConnections = new();
    public int leafCount;
}