using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    private float _horizontalBoundary = 11.3f;
    private float _verticalBoundary = 6.25f;
    private float _spawnRate = 4f;

    private Player _player;

    private Animator _animator;

    [SerializeField]
    private AudioClip _expolseAudioClip;

    private AudioSource _audioSource;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private float _fireDelay = 2f;

    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;

    [System.Obsolete]
    void Start()
    {
        _player = FindObjectOfType<Player>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        if(_player == null)
        {
            Debug.LogError("Player is null");
        }

        if(_animator == null)
        {
            Debug.LogError("Animator is null");
        }

        if(_audioSource == null)
        {
            Debug.LogError("Audio Source in player is null");
        } else
        {
            _audioSource.clip = _expolseAudioClip;
        }
    }

    void Update()
    {
        MoveEnemy();
        MoveLaserDown();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_player != null)
            {
                _player.Damage();
            }

            _audioSource.Play();
            _animator.SetTrigger("OnEnemyExplode");
            _spawnRate = 0f;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.5f);
        }
        if (other.CompareTag("Laser"))
        {
            if (_player != null)
            {
                _player.updatePlayerScore(1);
            }
            _audioSource.Play();
            _animator.SetTrigger("OnEnemyExplode");
            _spawnRate = 0f;
            Destroy(other.gameObject);
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.5f);
        }
    }

    void MoveLaserDown()
    {
        if(Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position + Vector3.down * 0.5f, Quaternion.identity);

            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i< lasers.Length; i++)
            {
                lasers[i].ActiveEnemyLaser();
            }
        }
    }

    void MoveEnemy()
    {
        transform.Translate(Vector3.down * _spawnRate * Time.deltaTime);

        if (transform.position.y < -_verticalBoundary)
        {
            Vector3 spawnPosition = new Vector3(
                Random.Range(-_horizontalBoundary, _horizontalBoundary),
                _verticalBoundary,
                0
            );

            transform.position = spawnPosition;
        }
    }
}