using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public float secondsBetweenCoins = 2f;

    // Use this for initialization
    private void Start()
    {
        InvokeRepeating("SpawnCoin", 0, secondsBetweenCoins);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void SpawnCoin()
    {
        var x = Random.Range(-20f, 20f);
        var z = Random.Range(-20f, 20f);
        Instantiate(coinPrefab, new Vector3(x, z, 0.5f), Quaternion.identity);
    }
}