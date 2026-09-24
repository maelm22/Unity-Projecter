using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoFlyweight
{
    public class Tile : MonoBehaviour
    {
        public Terrain type;
        public int movementCost;

        // Start is called before the first frame update
        void Start()
        {
            Texture2D tx = new Texture2D(256, 256);
            Sprite sp = Sprite.Create(tx, new Rect(0.0f, 0.0f, tx.width, tx.height), new Vector2(0.5f, 0.5f), 256);
            GetComponent<SpriteRenderer>().sprite = sp;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
