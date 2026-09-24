using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Powerup
{
    Sphere, Cube, Sphere2
}

/// <summary>
/// The factory that we can use to create our powerups-prefab instances
/// </summary>
public static class PowerupFactory
{
    static GameObject[] prefabs;

    /// <summary>
    /// Loads the prefabs from the "Resources/Prefabs/Powerups" folder in the project
    /// </summary>
    static void LoadPrefabs()
    {
        prefabs = Resources.LoadAll<GameObject>("Prefabs/Powerups");
    }

    /// <summary>
    /// Creates a random object from the prefabs array
    /// </summary>
    /// <returns></returns>
    public static GameObject CreateRandom()
    {
        //if prefabs is empty (which means it's the first time we ask to create something since the start of the game), we load the prefabs
        if (prefabs == null)
            LoadPrefabs();
        //if the array of prefabs is not empty we can spawn a random one and return it to the caller
        if (prefabs.Length != 0)
            return GameObject.Instantiate(prefabs[Random.Range(0, prefabs.Length)]);
        else
        {
            Debug.LogError("No prefabs in the folder!");
            return null;
        }
    }

    /// <summary>
    /// Creates and returns a specific type of object
    /// </summary>
    /// <param name="powerup"></param>
    /// <returns></returns>
    public static GameObject Create(Powerup powerup)
    {
        if (prefabs == null)
            LoadPrefabs();
        switch (powerup)
        {
            case Powerup.Sphere2:
                return GameObject.Instantiate(prefabs[2]);
            case Powerup.Sphere:
                return GameObject.Instantiate(prefabs[1]);
            case Powerup.Cube:
                return GameObject.Instantiate(prefabs[0]);
            default:
                return null;
        }
    }

}


