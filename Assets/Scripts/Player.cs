using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _cooldown = 0.5f;
    private float _canFire = 0f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;

    void Start()
    {
        transform.position = new Vector3(0, 2, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }
    }
    void Update()
    {
        CalculateMovement();
        PrimaryAttack();
    }

    void PrimaryAttack()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            _canFire = Time.time + _cooldown;
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.63f, 0), Quaternion.identity);
        }
    }
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.9f, 0), transform.position.z);

        if (transform.position.x >= 11.4f)
        {
            transform.position = new Vector3(-11.4f, transform.position.y, transform.position.z);
        }
        else if (transform.position.x <= -11.4f)
        {
            transform.position = new Vector3(11.4f, transform.position.y, transform.position.z);
        }
    }
    
    public void Damage()
    {
        _lives = _lives - 1;

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);   
        }
    }
}