using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public float spawnInterval = 2f; // tiempo entre monedas
    public int maxCoins = 20;        // máximo de monedas simultáneas

    public float minX = -48f;
    public float maxX = 48f;
    public float minY = -48f;
    public float maxY = 48f;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            TrySpawnCoin();
        }
    }

    void TrySpawnCoin()
    {
        int currentCoins = GameObject.FindGameObjectsWithTag("Coin").Length;

        if (currentCoins < maxCoins)
        {
            float x = Random.Range(minX, maxX);
            float y = Random.Range(minY, maxY);

            Vector3 position = new Vector3(x, y, 0);
            Instantiate(coinPrefab, position, Quaternion.identity);
        }
    }
}
