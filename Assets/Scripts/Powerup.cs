using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _powerupSpeed = 3f;
    [SerializeField] //0=TripleShot, 1=SpeedBoost, 2=Shield
    private int _powerupID;

    [SerializeField] AudioClip _powerupCollected;
    private AudioSource _audioSource;

    private SpriteRenderer _spriteRenderer;
    private PolygonCollider2D _collider;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Poperup Audio Source component is NULL.");
        }
        else
        {
            _audioSource.clip = _powerupCollected;
        }

        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
        {
            Debug.LogError("Powerup Sprite Renderer component is null.");
        }

        _collider = GetComponent<PolygonCollider2D>();
        if (_collider == null)
        {
            Debug.LogError("Powerup Polygon Collider 2D component is null.");
        }
    }
    void Update()
    {
        transform.Translate(_powerupSpeed * new Vector3(0, -1f, 0) * Time.deltaTime);
        if (transform.position.y < -10)
        {
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
                switch(_powerupID)
                {
                    case 0:
                        player.ActivateTripleShot();
                        Debug.Log("TRIPLE SHOT GONNA BURN YOU DOWN!");
                        break;
                    case 1:
                        player.ActivateSpeedBoost();
                        Debug.Log("SPEED BOOST, BOYYYYY!");
                        break;
                    case 2:
                        player.ActivateShield();
                        Debug.Log("SHIELDS, SUCKERS!");
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }
            }
            _audioSource.Play();
            _spriteRenderer.enabled = !_spriteRenderer.enabled;
            _collider.enabled = !_collider.enabled;
            Destroy(this.gameObject, 2.0f);
        }
    }
}   