using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Random = UnityEngine.Random;

public class PowerupSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _powerUpPrefabs = new List<GameObject>();

    [Header("SpawnSettings")]
    [SerializeField] private float _minTimeToSpawn = 20f;
    [SerializeField] private float _maxTimeToSpawn = 70f;
    [SerializeField] private Vector2 _horizScreenBounds = Vector2.zero;
    [SerializeField] private Vector2 _vertiScreenBounds = Vector2.zero;
    
    private void Start()
    {
        StartCoroutine(SpawnPowerups());
    }

    private IEnumerator SpawnPowerups()
    {
        while (true)
        {
            var timing = Random.Range(_minTimeToSpawn, _maxTimeToSpawn);
            yield return new WaitForSeconds(timing);

            var position = Vector3.zero;
            var x = Random.Range(_horizScreenBounds.x, _horizScreenBounds.y);
            var y = Random.Range(_vertiScreenBounds.x, _vertiScreenBounds.y);

            position.x = x;
            position.y = y;

            var index = Random.Range(0, _powerUpPrefabs.Count);
            var powerUp = _powerUpPrefabs[index];
            
            Instantiate(powerUp, position, Quaternion.identity, null);
        }
    }

}