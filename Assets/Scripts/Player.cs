using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private float _speedBoostMultiplier;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _shieldVisualizer;
    [SerializeField] private float _cooldown = 0.12f;
    private float _canFire = 0f;
    private bool _canMove = true;
    [SerializeField] private int _lives = 3;
    [SerializeField] private bool _tripleShotActive;
    [SerializeField] private bool _speedBoostActive;
    [SerializeField] private bool _shieldActive;
    [SerializeField] private bool _thrusterActive;
    [SerializeField] private int _score;

    [SerializeField] private AudioClip _laserSoundClip;
    [SerializeField] private AudioClip _explosion;
 
    private UIManager _uiManager;
    private SpawnManager _spawnManager;
    private AudioSource _audioSource;
    private Animator _anim;
    [SerializeField] private GameObject _leftEngine, _rightEngine;
    [SerializeField] private GameObject _thruster;
    
    void Start()
    {
        transform.position = new Vector3(0, -3.5f, 0);

        _thruster.SetActive(true);
        if (_thruster == null)
        {
            Debug.LogError("Thruster is NULL.");
        }

        if (_rightEngine == null)
        {
            Debug.LogError("Right_Engine is NULL.");
        }
        _rightEngine.SetActive(false);

        if (_leftEngine == null)
        {
            Debug.LogError("Left_Engine is NULL.");
        }     
        _leftEngine.SetActive(false);

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("The UIManager is NULL.");
        }

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Player's AudioSource is NULL.");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("The Player's Animator component is NULL.");
        }
    }
    void Update()
    {
        CalculateMovement();
        PrimaryAttack();
        ShieldVisualizer();
    }
    void PrimaryAttack()
    {
        if (_tripleShotActive == false && Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            _canFire = Time.time + _cooldown;
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.63f, 0), Quaternion.identity);
            _audioSource.Play();
        }

        else if (_tripleShotActive == true && Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            _canFire = Time.time + _cooldown;
            Instantiate(_tripleShotPrefab, transform.position + new Vector3(0, 0.63f, 0), Quaternion.identity);
            _audioSource.Play();
        }
    }
    void CalculateMovement()
    {
        if (_canMove == true)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
            transform.Translate(direction * _speed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.9f, 6.0f), transform.position.z);

            if (transform.position.x >= 11.4f)
            {
                transform.position = new Vector3(-11.4f, transform.position.y, transform.position.z);
            }
            else if (transform.position.x <= -11.4f)
            {
                transform.position = new Vector3(11.4f, transform.position.y, transform.position.z);
            }
        }
    }

    void ShieldVisualizer()
    {
        if (_shieldActive == true)
        {
            _shieldVisualizer.SetActive(true);
        }
    }
    public void Damage()
    {
        if (_shieldActive == true)
        {
            _shieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }
        _lives = _lives - 1;

        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
        }

        if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            _anim.SetTrigger("OnPlayerDeath");
            _audioSource.clip = _explosion;
            _canMove = false;
            _audioSource.Play();
            _thruster.SetActive(false);
            _rightEngine.SetActive(false);
            _leftEngine.SetActive(false);
            Destroy(this.gameObject, 2.5f);
        }
    }
    public void ActivateTripleShot()
    {
        _tripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    public void ActivateSpeedBoost()
    {
        _speedBoostActive = true;
        _speed *= _speedBoostMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    public void ActivateShield()
    {
        _shieldActive = true;
        _shieldVisualizer.gameObject.SetActive(true);
    }
    IEnumerator TripleShotPowerDownRoutine()
    {
        while (_tripleShotActive == true)
            {
            yield return new WaitForSeconds(5.0f);
            _tripleShotActive = false;
            }
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        while (_speedBoostActive == true)
        {
            yield return new WaitForSeconds(8.0f);
            _speedBoostActive = false;
            _speed /= _speedBoostMultiplier;
        }
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}