using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Singleton;

    [Header("Spawn Settings")] [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField] private GameObject _shootingEnemyPrefab;
    [SerializeField] private GameObject _bossPrefab;
    [SerializeField] private float _shootingEnemySpawnChance = 40f; //Chance out of 100 for a shooting enemy to spawn.
    [SerializeField] private float _minTimeToSpawn = 1f;
    [SerializeField] private float _maxTimeToSpawn = 5f;
    [SerializeField] private Vector2 _horizScreenBounds = Vector2.zero;
    [SerializeField] private Vector2 _vertiScreenBounds = Vector2.zero;
    [SerializeField] private GameObject _spawnWarning;

    [SerializeField] public bool BossSpawned = false;

    [SerializeField] private int _bossesSpawned = 0;

    private float _timeSinceLastSpawn;


    public void Start()
    {
        if (Singleton != null)
        {
            Destroy(this);
        }

        Singleton = this;

        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnBosses());
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

    private IEnumerator SpawnEnemyWithWarning(GameObject enemy, Vector3 position)
    {
        var warning = Instantiate(_spawnWarning, position, Quaternion.identity, null);
        yield return new WaitForSeconds(0.4f);
        Destroy(warning);
        yield return new WaitForSeconds(0.1f);
        Instantiate(enemy, position, Quaternion.identity, null);
    }

    private IEnumerator SpawnBosses() {
        while (true)
        {
            if (Player.Singleton.PlayerScore != 0 && Player.Singleton.PlayerScore % 60 == 0 && !BossSpawned)
            {

                for (var i = 0; i < _bossesSpawned + 1; i++)
                {
                    var spawnPos = Vector3.zero;
                    var x = Random.Range(_horizScreenBounds.x, _horizScreenBounds.y);
                    var y = Random.Range(_vertiScreenBounds.x, _vertiScreenBounds.y);
                    spawnPos.x = x;
                    spawnPos.y = y;

                    StartCoroutine(SpawnEnemyWithWarning(_bossPrefab, spawnPos));

                    BossSpawned = true;
                }

                _bossesSpawned++;
            }

            yield return new WaitForSeconds(1.0f);
        }
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
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

                StartCoroutine(SpawnEnemyWithWarning(prefab, spawnPos));
            }
            
            var timeToWait = Random.Range(_minTimeToSpawn, _maxTimeToSpawn);
            yield return new WaitForSeconds(timeToWait);
            
        }
    }
}