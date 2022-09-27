using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 5.0f;

    void Start()
    {
    
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
        transform.Translate(_enemySpeed * new Vector3(0, -1f, 0) * Time.deltaTime);
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

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}

