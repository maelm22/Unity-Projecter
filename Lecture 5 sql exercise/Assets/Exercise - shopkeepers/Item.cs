using System;

/// <summary>
/// The class to contain the info for each item
/// </summary>
[Serializable]
public class Item
{
    public string name;
    public int cost;

    public Item(string name, int cost)
    {
        this.name = name;
        this.cost = cost;
    }
}
