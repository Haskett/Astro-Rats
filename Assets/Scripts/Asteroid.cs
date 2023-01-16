using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 36.0f;
    private Player _player;
    private Animator _anim;
    private PolygonCollider2D _collider;
    private AudioSource _audioSource;
    [SerializeField] private GameObject _littleAsteroid;
    [SerializeField] private GameObject _obstacleContainer;

    [SerializeField] private AudioClip _explosion;

    void Start()
    {
        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("The AsteroidAnimator is NULL");
        }

        _player = GameObject.Find("Player").GetComponent<Player>();     
        if (_player == null)
        {
            Debug.LogError("The Player is NULL.");
        }

        _collider = GetComponent<PolygonCollider2D>();
        if (_collider == null)
        {
            Debug.LogError("The Enemy PolygonCollider2D component is NULL.");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("The Asteroid AudioSource component is NULL.");
        }
        else
        {
            _audioSource.clip = _explosion;
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
                _player.AddScore(50);
            }
            _anim.SetTrigger("OnAsteroidDestruction");
            _audioSource.Play();
            _collider.enabled = !_collider.enabled;
            Instantiate(_littleAsteroid, transform.position, Quaternion.identity);
            Instantiate(_littleAsteroid, transform.position, Quaternion.identity);
            Instantiate(_littleAsteroid, transform.position, Quaternion.identity);
            Instantiate(_littleAsteroid, transform.position, Quaternion.identity);
            Destroy(this.gameObject, 2.5f);
            Destroy(transform.parent.gameObject, 2.5f);
        }

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            _anim.SetTrigger("OnAsteroidDestruction");
            _audioSource.Play();
            _collider.enabled = !_collider.enabled;
            Destroy(this.gameObject, 2.5f);
            Destroy(transform.parent.gameObject, 2.5f);
        }
    }
}
