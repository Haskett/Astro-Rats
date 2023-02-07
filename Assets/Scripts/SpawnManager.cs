using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _spawnedEnemyPrefab1;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject _obstacleContainer;
    [SerializeField] private GameObject _spawnedAsteroid;
    [SerializeField] private GameObject[] powerups;
    private bool _stopSpawning = false;
    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnAsteroidRoutine());
        StartCoroutine(SpawnAmmo());
    }
    IEnumerator SpawnEnemyRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 10, 0);
            GameObject newEnemy = Instantiate(_spawnedEnemyPrefab1, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(4.6f);
        }
    }

    IEnumerator SpawnAsteroidRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 posToSpawnAsteroid = new Vector3(Random.Range(-11.0f, 11.0f), 12);
            GameObject newAsteroid = Instantiate(_spawnedAsteroid, posToSpawnAsteroid, Quaternion.identity);
            newAsteroid.transform.parent = _obstacleContainer.gameObject.transform;
            yield return new WaitForSeconds(20.0f);
        }
    }
    IEnumerator SpawnPowerupRoutine()
    {
        while (_stopSpawning == false)
        {
            GameObject _powerupContainer = transform.GetChild(4).gameObject;
            if (_powerupContainer == null)
            {
                Debug.LogError("Spawn Manager's Powerup Container is NULL.");
            }
            
            Vector3 posToSpawnTripleShot = new Vector3(Random.Range(-8f, 8f), 10, 0);
            int randomPowerUp = Random.Range(0, 5);
            GameObject _newPowerup = Instantiate(powerups[randomPowerUp], posToSpawnTripleShot, Quaternion.identity);
            _newPowerup.transform.parent = _powerupContainer.transform;
            
            yield return new WaitForSeconds(Random.Range(8f, 12f));
        }
    }

    IEnumerator SpawnAmmo()
    {
        while (_stopSpawning == false)
        {
            GameObject _powerupContainer = transform.GetChild(4).gameObject;           
            if (_powerupContainer == null)
            {
                Debug.LogError("Spawn Manager's Powerup Container is NULL.");
            }

            Vector3 posToSpawnAmmo = new Vector3(Random.Range(-8f, 8f), 10, 0);
            GameObject _newPowerup = Instantiate(powerups[3], posToSpawnAmmo, Quaternion.identity);
            _newPowerup.transform.parent = _powerupContainer.transform;
            yield return new WaitForSeconds(Random.Range(2f, 6f));
        }
    }
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}