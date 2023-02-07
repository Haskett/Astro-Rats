using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidLittle : MonoBehaviour
{
    private Animator _anim;
    private PolygonCollider2D _collider;
    private AudioSource _audioSource;
    private SpawnManager _spawnManager;
    [SerializeField] private AudioClip _explosion;
    private Player _player;
    [SerializeField] private float _rotationSpeed = 36.0f;

    void Start()
    {
        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("The Mini Asteroid Animator is NULL.");
        }

        _collider = GetComponent<PolygonCollider2D>();
        if (_collider == null)
        {
            Debug.LogError("The Mini Asteroid PolygonCollider2D is NULL.");
        }

        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("The Player is NULL.");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_explosion == null)
        {
            Debug.LogError("The LittleAsteroid AudioSource is NULL.");
        }
        else
        {
            _audioSource.clip = _explosion;
        }

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * _rotationSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(25);
            }
            _anim.SetTrigger("OnAsteroidDestruction");
            _audioSource.Play();
            _collider.enabled = !_collider.enabled;
            Destroy(this.gameObject, 2.5f);
            Destroy(transform.parent.gameObject, 2.5f);
        }

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                _anim.SetTrigger("OnAsteroidDestruction");
                _collider.enabled = !_collider.enabled;
                player.Damage();
            }
        }

        if (other.tag == "Enemy")
        {
            _anim.SetTrigger("OnAsteroidDestruction");
            _collider.enabled = !_collider.enabled;
            Destroy(this.gameObject, 2.5f);
            Destroy(transform.parent.gameObject, 2.5f);
        }
    }
}
