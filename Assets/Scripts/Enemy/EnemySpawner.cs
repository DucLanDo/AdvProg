using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public GameObject enemyPrefab;
    private float spawnTimer;
    public float spawnInterval;


    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval) {
            spawnTimer = 0;
            SpawnEnemy();
        }
    }
    private void SpawnEnemy() {
        Instantiate(enemyPrefab, transform.position, transform.rotation);
    }
}
