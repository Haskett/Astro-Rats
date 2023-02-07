using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AsteroidLittleContainer : MonoBehaviour
{
    [SerializeField] private float _miniAsteroidSpeed = 2.0f;
    private SpawnManager _spawnManager;
    private Vector3 _randomVector3;

    void Start()
    {
        _randomVector3 = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f));
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }
    }


    void Update()
    {
        transform.Translate(_miniAsteroidSpeed * Time.deltaTime * _randomVector3);
        
        if (transform.position.x > 22 || transform.position.x < -22 || transform.position.y > 12 || transform.position.y < -12)
        {
            Destroy(this.gameObject);
        }
    }
}
