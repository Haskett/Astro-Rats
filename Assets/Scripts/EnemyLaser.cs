using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField] private float _laserSpeed = 10.0f;

    private Player _player;
    private BoxCollider2D _collider;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("The Player is Null.");
        }

        _collider = GetComponent<BoxCollider2D>();
        if (_collider == null)
        {
            Debug.LogError("The Enemy Laser BoxCollider2D is NULL.");
        }

    }
    void Update()
    {
        transform.Translate(Vector3.down * _laserSpeed * Time.deltaTime);
        if (transform.position.y <= -8.0f)
        {
            //if (transform.parent != null)
            //{
            //    Destroy(transform.parent.gameObject);
            //}
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            Destroy(this.gameObject);
        }
    }
}
