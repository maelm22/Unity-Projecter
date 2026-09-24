using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WithFlyweight
{
    public class Tile 
    {
        public Terrain type;
        public int movementCost;

        public Tile (Terrain type, int movementCost)
        {
            this.type = type;
            this.movementCost = movementCost;
        }
    }
}
