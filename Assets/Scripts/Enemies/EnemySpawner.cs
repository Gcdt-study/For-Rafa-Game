using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int numberOfEnemies = 5;

    public float minX = -48f;
    public float maxX = 48f;
    public float minY = -48f;
    public float maxY = 48f;

    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            float x = Random.Range(minX, maxX);
            float y = Random.Range(minY, maxY);

            Vector3 position = new Vector3(x, y, 0);
            Instantiate(enemyPrefab, position, Quaternion.identity);
        }
    }
}
