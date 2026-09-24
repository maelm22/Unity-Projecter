using UnityEngine;
using System;
using System.Collections.Generic;       //Allows us to use Lists.
using Random = UnityEngine.Random;      //Tells Random to use the Unity Engine random number generator.


public class BoardManager : MonoBehaviour
{
    // Using Serializable allows us to embed a class with sub properties in the inspector.
    [Serializable]
    public class Count
    {
        public int minimum;             //Minimum value for our Count class.
        public int maximum;             //Maximum value for our Count class.


        //Assignment constructor.
        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }


    public int columns = 8;                                         //Number of columns in our game board.
    public int rows = 8;                                            //Number of rows in our game board.
    public Count wallCount = new Count(5, 9);                      //Lower and upper limit for our random number of walls per level.
    public Count foodCount = new Count(1, 5);                      //Lower and upper limit for our random number of food items per level.
    public GameObject exit;                                         //Prefab to spawn for exit.
    public GameObject[] floorTiles;                                 //Array of floor prefabs.
    public GameObject[] wallTiles;                                  //Array of wall prefabs.
    public GameObject[] foodTiles;                                  //Array of food prefabs.
    public GameObject[] enemyTiles;                                 //Array of enemy prefabs.
    public GameObject[] outerWallTiles;                             //Array of outer tile prefabs.

    private Transform boardHolder;                                  //A variable to store a reference to the transform of our Board object.
    private List<Vector3> gridPositions = new List<Vector3>();   //A list of possible locations to place tiles.

    public Algorithms algorithm = Algorithms.Random;

    public enum Algorithms
    {
         Random,
         RandomWalk,
         SpelunkyStyle
    };

    //Clears our list gridPositions and prepares it to generate a new board.
    void InitialiseList()
    {
        //Clear our list gridPositions.
        gridPositions.Clear();

        //Loop through x axis (columns).
        for (int x = 1; x < columns - 1; x++)
        {
            //Within each column, loop through y axis (rows).
            for (int y = 1; y < rows - 1; y++)
            {
                //At each index add a new Vector3 to our list with the x and y coordinates of that position.
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }


    //Sets up the outer walls and floor (background) of the game board.
    void BoardSetup()
    {
        //Instantiate Board and set boardHolder to its transform.
        boardHolder = new GameObject("Board").transform;

        //Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
        for (int x = -1; x < columns + 1; x++)
        {
            //Loop along y axis, starting from -1 to place floor or outerwall tiles.
            for (int y = -1; y < rows + 1; y++)
            {
                //Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

                //Check if we current position is at board edge, if so choose a random outer wall prefab from our array of outer wall tiles.
                if (x == -1 || x == columns || y == -1 || y == rows)
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];

                //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
                GameObject instance =
                    Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                //Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
                instance.transform.SetParent(boardHolder);
            }
        }
    }


    //RandomPosition returns a random position from our list gridPositions.
    Vector3 RandomPosition()
    {

        //Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
        int randomIndex = Random.Range(0, gridPositions.Count);

        //Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
        Vector3 randomPosition = gridPositions[randomIndex];

        //Remove the entry at randomIndex from the list so that it can't be re-used.
        gridPositions.RemoveAt(randomIndex);

        //Return the randomly selected Vector3 position.
        return randomPosition;
    }


    //LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {

        //Choose a random number of objects to instantiate within the minimum and maximum limits
        int objectCount = Random.Range(minimum, maximum + 1);

        //Instantiate objects until the randomly chosen limit objectCount is reached
        for (int i = 0; i < objectCount; i++)
        {
            //if there is no space return
            if (gridPositions.Count == 0)
                return;

            //Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
            Vector3 randomPosition = RandomPosition();

            //Choose a random tile from tileArray and assign it to tileChoice
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];

            //Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }


    //SetupScene initializes our level and calls the previous functions to lay out the game board
    public void SetupScene(int level)
    {
        //Creates the outer walls and floor.
        BoardSetup();

        //Reset our list of gridpositions.
        InitialiseList();

        //Instantiate a random number of wall tiles based on minimum and maximum, at randomized positions.
        if (algorithm == Algorithms.Random)
            LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        if (algorithm == Algorithms.SpelunkyStyle)
            SpelunkyLikeWalls(wallTiles);
        if (algorithm == Algorithms.RandomWalk)
            RandomWalkWalls(wallTiles);

        //Instantiate a random number of food tiles based on minimum and maximum, at randomized positions.
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);

        //Determine number of enemies based on current level number, based on a logarithmic progression
        int enemyCount = (int)Mathf.Log(level, 2f);

        //Instantiate a random number of enemies based on minimum and maximum, at randomized positions.
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);

        //Instantiate the exit tile in the upper right hand corner of our game board
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }

    //creates walls by making a constrained walk in the level (throw a 5 sided dice: 1-2 go left, 3-4 go right, 5 go up)
    void SpelunkyLikeWalls(GameObject[] wallTiles)
    {

        //the agent starts at (0,0)
        int agentX = 0;
        int agentY = 0;
        //in this list we save the visited nodes in the path
        List<Vector3> path = new List<Vector3>();
        //The CarveHole function "makes a hole" in the specified position and saves it in the path list
        CarveHole(agentX, agentY, ref path);
        
        
        
        while (!(agentX == rows - 1  && agentY == columns - 1))
        {
            int die = (int)Random.Range(1, 5f);

            switch (die)
            {
                case 1:
                case 2:
                {
                    if (agentX > 0)
                    {
                       agentX--;
                    }
                    
                    break;
                }
                case 3:
                case 4:
                {
                    if (agentX < rows - 1)
                    {
                        agentX++;
                    }
                    
                    break;
                }

                case 5:
                {

                    if (agentY < columns - 1)
                    {
                        agentY++;
                    }

                    break;
                }
            }
            CarveHole(agentX,agentY,ref path);
              
        }
        
        
        /*
         * IMPLEMENT SPELUNKY-LIKE PATH CONSTRUCTION HERE
         */

        //remove all gridPositions, as we are starting with a completely filled map (instead of the completely empty one from the tutorial example)
        gridPositions.Clear();
        //we make the gridPositions the same as the path, remember that gridPositions is the places where items/monsters can spawn
        gridPositions = path;

        //Finally we fill all positions not part of path with walls
        CarveEntirePath(path);
    }

   

    //creates a path by randomly moving through the level
    void RandomWalkWalls(GameObject[] wallTiles)
    {
        //the agent starts at (0,0)
        int agentX = 0;
        int agentY = 0;
        //in this list we save the visited nodes in the path
        List<Vector3> path = new List<Vector3>();
        //The CarveHole function "makes a hole" in the specified position and saves it in the path list
        CarveHole(agentX, agentY, ref path);
        
        
        
        
        /*
         * IMPLEMENT RANDOM WALK HERE
         */
        while (!(agentX == rows - 1  && agentY == columns - 1))
        {
            int breakeout = 0;
            
            int die = (int)Random.Range(1, 4f);
            
            switch (die)
            {
                case 1:
                {
                    if (agentX > 0)
                    {
                        agentX--;
                    }
                        
                    break;
                }

                case 2:
                {
                    if (agentX < rows - 1)
                    {
                        agentX++;
                    }
                                        
                    break;
                }
                
                case 3:
                {
                    if (agentY > 0)
                    {
                        agentY--;
                    }
                    break;
                }
                
                case 4:
                {
                     if (agentY < columns - 1)
                     {
                         agentY++;
                     }
                    
                     break;
                }
                    
                
            }
            CarveHole(agentX,agentY,ref path);
            if (breakeout == 1000)
            { ;
            }
            
            breakeout++;
            
        
        }
        

        //remove all gridPositions, as we are starting with a completely filled map (instead of the completely empty one from the tutorial example)
        gridPositions.Clear();
        //we make the gridPositions the same as the path, remember that gridPositions is the places where items/monsters can spawn
        gridPositions = path;

        //Finally we fill all positions not part of path with walls
        CarveEntirePath(path);
    }

    //this function checks if the position we're visiting has already been carved (is already in path)
    //if not, it adds it to the path
    void CarveHole(int x, int y, ref List<Vector3> path)
    {
        Vector3 newPosition = new Vector3(x, y, 0);
        if (!path.Contains(newPosition))
            path.Add(newPosition);
    }

    //this function fills the level by adding a random inner wall tile in every tile that is not in the calculated path
    void CarveEntirePath(List<Vector3> path)
    {
        //Fill in the level
        for (int x = 0; x < columns; x++)
        {
            //Loop along y axis, starting from -1 to place floor or outerwall tiles.
            for (int y = 0; y < rows; y++)
            {
                Vector3 currentPosition = new Vector3(x, y, 0);
                if (!path.Contains(currentPosition))
                {
                    GameObject tileChoice;
                    tileChoice = wallTiles[Random.Range(0, wallTiles.Length)];

                    //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
                    GameObject instance =
                        Instantiate(tileChoice, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                    //Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
                    instance.transform.SetParent(boardHolder);
                }

            }
        }
    }
}