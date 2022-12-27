using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class RandomSpawner : MonoBehaviour
    {
        [SerializeField] private List<GameObject> enemyPrefabs;
        [SerializeField] private List<int> enemySpawnRates;

        [SerializeField] private GameObject spawnPoint;
        [SerializeField] private Collider2D spawnArea;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private float spawnDelay;

        private float _timeSinceLastSpawn;

        public static RandomSpawner Instance { get; private set; }

        private void Start()
        {
            Instance = this;
        }

        private void Update()
        {
            _timeSinceLastSpawn += Time.deltaTime;
            if (!(_timeSinceLastSpawn >= spawnDelay)) return;
            StartCoroutine(SpawnEnemy());
        }

        private IEnumerator SpawnEnemy()
        {
            var loop = true;
            var bounds = spawnArea.bounds;

            while (loop)
            {
                var randomEnemy = Random.Range(0, enemyPrefabs.Count);
                var randomSpawnRate = Random.Range(1, 100);

                if (randomSpawnRate > enemySpawnRates[randomEnemy]) continue;
                var randomPosition = new Vector3(Random.Range(bounds.min.x, bounds.max.x),
                    Random.Range(bounds.min.y, bounds.max.y), 0f);

                if (mainCamera.WorldToViewportPoint(randomPosition).x is >= 0f and <= 1f &&
                    mainCamera.WorldToViewportPoint(randomPosition).y is >= 0f and <= 1f) continue;
                Instantiate(enemyPrefabs[randomEnemy], randomPosition, Quaternion.identity, spawnPoint.transform);
                _timeSinceLastSpawn = 0f;
                loop = false;
            }

            yield return null;
        }
    }
}