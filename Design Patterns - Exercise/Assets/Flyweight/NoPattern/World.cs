using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoFlyweight
{
    public class World : MonoBehaviour
    {
        public GameObject[,] tiles;
        public GameObject grassPrefab;
        public GameObject hillPrefab;
        public GameObject riverPrefab;

        public int size = 5;

        // Start is called before the first frame update
        void Start()
        {
            tiles = new GameObject[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    switch (Random.Range(0, 3))
                    {
                        case 0:
                            tiles[i, j] = Instantiate(grassPrefab, new Vector3(i, j, 0), Quaternion.identity);
                            break;
                        case 1:
                            tiles[i, j] = Instantiate(hillPrefab, new Vector3(i, j, 0), Quaternion.identity);
                            break;
                        case 2:
                            tiles[i, j] = Instantiate(riverPrefab, new Vector3(i, j, 0), Quaternion.identity);
                            break;
                    }
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
