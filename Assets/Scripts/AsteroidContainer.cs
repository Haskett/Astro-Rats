using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidContainer : MonoBehaviour
{
    private Vector3 _asteroidPath;
    // [SerializeField] private GameObject _asteroid;

    void Start()
    {
        _asteroidPath = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-3.0f, -2.0f));
    }

    void Update()
    {
        transform.Translate(_asteroidPath * Time.deltaTime);

        if (transform.position.x > 22 || transform.position.x < -22 || transform.position.y > 12 || transform.position.y < -12)
        {
            Destroy(this.gameObject);
        }
    }
}
