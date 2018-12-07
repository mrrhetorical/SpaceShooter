using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Singleton;

    [Header("Spawn Settings")]
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _shootingEnemyPrefab;
    [SerializeField] private GameObject _bossPrefab;
    [SerializeField] private float _shootingEnemySpawnChance = 40f; //Chance out of 100 for a shooting enemy to spawn.
    [SerializeField] private float _minTimeToSpawn = 1f;
    [SerializeField] private float _maxTimeToSpawn = 5f;
    [SerializeField] private Vector2 _horizScreenBounds = Vector2.zero;
    [SerializeField] private Vector2 _vertiScreenBounds = Vector2.zero;
    
    [SerializeField] public bool BossSpawned = false;
    
    private float _timeSinceLastSpawn;
   
   
    public void Start()
    {
        if (Singleton != null)
        {
            Destroy(this);
        }

        Singleton = this;

        StartCoroutine(SpawnEnemies());
        StartCoroutine(IncreaseDifficulty());
    }

    private IEnumerator IncreaseDifficulty()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            _maxTimeToSpawn -= 0.02f;
            if (_maxTimeToSpawn < 1f)
            {
                _minTimeToSpawn -= 0.01f;
                if (_minTimeToSpawn < 0.2f)
                {
                    _minTimeToSpawn = 0.2f;
                }

                _maxTimeToSpawn = 1f;
            }
        }
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (Player.Singleton.PlayerScore > 10 && Player.Singleton.PlayerScore % 60 <= _maxTimeToSpawn && !BossSpawned)
            {
                var spawnPos = Vector3.zero;
                var x = Random.Range(_horizScreenBounds.x, _horizScreenBounds.y);
                var y = Random.Range(_vertiScreenBounds.x, _vertiScreenBounds.y);
                spawnPos.x = x;
                spawnPos.y = y;
                
                Instantiate(_bossPrefab, spawnPos, Quaternion.identity, null);

                BossSpawned = true;
            }

            if (!BossSpawned)
            {
                var spawnPos = Vector3.zero;
                var x = Random.Range(_horizScreenBounds.x, _horizScreenBounds.y);
                var y = Random.Range(_vertiScreenBounds.x, _vertiScreenBounds.y);
                spawnPos.x = x;
                spawnPos.y = y;

                GameObject prefab;

                var chance = Random.Range(0, 100);
                if (chance <= _shootingEnemySpawnChance)
                {
                    prefab = _shootingEnemyPrefab;
                }
                else
                {
                    prefab = _enemyPrefab;
                }

                Instantiate(prefab, spawnPos, Quaternion.identity, null);
            }
            
            var timeToWait = Random.Range(_minTimeToSpawn, _maxTimeToSpawn);
            yield return new WaitForSeconds(timeToWait);
            
        }
    }
}