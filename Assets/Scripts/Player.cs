using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 3.5f;
    private float _shiftSpeedBoost = 1.0f;
    [SerializeField] private float _heldShiftSpeedBoost = 2.0f;
    private bool _canMove = true;

    [SerializeField] private float _speedBoostPowerupMultiplier;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;

    [SerializeField] private GameObject _shieldVisualizer2;
    [SerializeField] private GameObject _shieldVisualizer1;
    [SerializeField] private GameObject _shieldVisualizer0;

    [SerializeField] private float _cooldown = 0.12f;
    private float _canFire = 0f;
    public int _ammoCount = 15;
    private bool _hasAmmo;

    [SerializeField] private int _lives = 3;
    [SerializeField] private int _shieldLives = 3;

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

        _hasAmmo = true;

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

        _shieldVisualizer2.SetActive(false);
        _shieldVisualizer1.SetActive(false);
        _shieldVisualizer0.SetActive(false);
    }
    void Update()
    {
        CalculateMovement();
        PrimaryAttack();
        //ShieldVisualizer();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _shiftSpeedBoost = _heldShiftSpeedBoost;
        }
        else
        {
            _shiftSpeedBoost = 1.0f;
        }
    }
    void PrimaryAttack()
    {
        if (_ammoCount < 1)
        {
            _hasAmmo = false;
        }
        else
        {
            _hasAmmo = true;
        }

        if (_tripleShotActive == false && Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _hasAmmo == true)
        {
            GameObject _playerLaserContainer = _spawnManager.transform.GetChild(3).gameObject;
            if (_playerLaserContainer == null)
            {
                Debug.LogError("Spawn Manager's Enemy Laser Container is NULL.");
            }

            _canFire = Time.time + _cooldown;
            _ammoCount--;
            _uiManager.UpdateAmmo(_ammoCount);
            GameObject newPlayerLaser = Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.63f, 0), Quaternion.identity);
            newPlayerLaser.transform.parent = _playerLaserContainer.transform;
            _audioSource.Play();
        }

        else if (_tripleShotActive == true && Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _hasAmmo == true)
        {
            GameObject _playerLaserContainer = _spawnManager.transform.GetChild(3).gameObject;
            if (_playerLaserContainer == null)
            {
                Debug.LogError("Spawn Manager's Enemy Laser Container is NULL.");
            }

            _canFire = Time.time + _cooldown;
            _ammoCount--;
            _uiManager.UpdateAmmo(_ammoCount);
            GameObject newPlayerTripleShot = Instantiate(_tripleShotPrefab, transform.position + new Vector3(0, 0.63f, 0), Quaternion.identity);
            newPlayerTripleShot.transform.parent = _playerLaserContainer.transform;
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
            transform.Translate(direction * _speed * Time.deltaTime * _shiftSpeedBoost);
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

    //void ShieldVisualizer()
    //{
    //    if (_shieldActive == true)
    //    {
    //        _shieldVisualizer2.SetActive(true);
    //        _shieldVisualizer1.SetActive(true);
    //        _shieldVisualizer0.SetActive(true);
    //    }
    //}

    public void Damage()
    {
        if (_shieldActive == true)
        {
            _shieldLives--;

            if (_shieldLives == 2)
            {
                _shieldVisualizer2.SetActive(false);
                _shieldVisualizer1.SetActive(true);
            }

            if (_shieldLives == 1)
            {
                _shieldVisualizer1.SetActive(false);
                _shieldVisualizer0.SetActive(true);
            }

            if (_shieldLives < 1)
            {
                _shieldVisualizer0.SetActive(false);
                _shieldActive = false;
            }
            return;
        }

        _lives--;

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
        _speed *= _speedBoostPowerupMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    public void ActivateShield()
    {
        if (_shieldActive == true)
        {
            _shieldLives = 3;
            _shieldVisualizer1.SetActive(false);
            _shieldVisualizer0.SetActive(false);
            _shieldVisualizer2.SetActive(true);
        }

        if (_shieldActive == false)
        {
            _shieldActive = true;
            _shieldVisualizer2.SetActive(true);
        }
    }

    public void AddAmmo()
    {
        _ammoCount += 7;
        _uiManager.UpdateAmmo(_ammoCount);
    }

    public void AddLife()
    {
        if (_lives < 3)
        {
            _lives++;
            _uiManager.UpdateLives(_lives);
        }

        else
        {
            _lives = 3;
        }
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
            _speed /= _speedBoostPowerupMultiplier;
        }
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}