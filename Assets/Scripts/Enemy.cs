using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _enemySpeed = 5.0f;

    private Player _player;
    private Animator _anim;
    private PolygonCollider2D _collider;
    private GameObject _spawnManager;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] AudioClip _explosion;
    private AudioSource _audioSource;
    private IEnumerator _enemyAttack;

    void Start()
    {
        NullCheck();
        _enemyAttack = EnemyAttackRoutine();
        StartCoroutine(_enemyAttack);
}

    void Update()
    {
        if (transform.position.y < -5.21)
        {
            transform.position = new Vector3(Random.Range(-9.7f, 9.7f), 7.3f, transform.position.z);
        }
        EnemyMovement();
    }
    void EnemyMovement()
    {
        transform.Translate(_enemySpeed * new Vector3(Mathf.Sin(transform.position.y), -1f, 0) * Time.deltaTime);
    }

    IEnumerator EnemyAttackRoutine()
    {
        while (_player != null)
        {
            GameObject _enemyLaserContainer = _spawnManager.transform.GetChild(2).gameObject;
            if (_enemyLaserContainer == null)
            {
                Debug.LogError("Spawn Manager's Enemy Laser Container is NULL.");
            }

            GameObject newEnemyLaser = Instantiate(_laserPrefab, transform.position + new Vector3(0, -0.63f, 0), Quaternion.identity);
            newEnemyLaser.transform.parent = _enemyLaserContainer.transform;
            yield return new WaitForSeconds(Random.Range(2.0f, 4.0f));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            StopCoroutine(_enemyAttack);
            if (_player != null)
            {
                _player.AddScore(10);
            }
            _anim.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _enemySpeed = 1.5f;
            _collider.enabled = !_collider.enabled;
            Destroy(this.gameObject, 2.25f);
        }

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            StopCoroutine(_enemyAttack);
            _anim.SetTrigger("OnEnemyDeath");
            _collider.enabled = !_collider.enabled;
            _audioSource.Play();
            _enemySpeed = 1.5f;
            Destroy(this.gameObject, 2.25f);
        }

        if (other.tag == "Asteroid")
        {
            StopCoroutine(_enemyAttack);
            _anim.SetTrigger("OnEnemyDeath");
            _collider.enabled = !_collider.enabled;
            _audioSource.Play();
            _enemySpeed = 1.5f;
            Destroy(this.gameObject, 2.25f);
        }
    }

    private void NullCheck()
    {
        _spawnManager = GameObject.Find("Spawn_Manager");
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL.");
        }

        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("The Player is NULL.");
        }

        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("The Enemy Animator is NULL.");
        }

        _collider = GetComponent<PolygonCollider2D>();
        if (_collider == null)
        {
            Debug.LogError("The Enemy PolygonCollider2D component is NULL.");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Enemy AudioSource is NULL.");
        }
        else
        {
            _audioSource.clip = _explosion;
        }
    }
}