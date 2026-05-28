using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private float _horizontalBoundary = 11.3f;
    private float _verticalBoundary = 6.25f;
    private float _spawnRate = 4f;
    private float _rotateAsteroid = 4f;

    private Player _player;

    private Animator _animator;

    [SerializeField]
    private AudioClip _expolseAudioClip;

    private AudioSource _audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [System.Obsolete]
    void Start()
    {
        _player = FindObjectOfType<Player>();
        _animator = GetComponent<Animator>();

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

    // Update is called once per frame
    void Update()
    {
        MoveAsteroidDown();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Asteroid collided with: " + collision.name);

        if(collision.CompareTag("Player"))
        {
            if (_player != null)
            {
                _player.Damage();
            }
            _audioSource.Play();
            _animator.SetTrigger("OnAsteroidDeath");
            _spawnRate = 0f;
            _rotateAsteroid = 0f;
            Destroy(this.gameObject, 2.5f);
        } else if (collision.CompareTag("Laser"))
        {
            _audioSource.Play();
            _animator.SetTrigger("OnAsteroidDeath");
            _spawnRate = 0f;
            _rotateAsteroid = 0f;
            Destroy(collision.gameObject);
            Destroy(this.gameObject, 2.5f);
        }

    }

    void MoveAsteroidDown()
    {
        transform.Translate(Vector3.down * _spawnRate * Time.deltaTime);
        transform.Rotate(Vector3.forward * _rotateAsteroid * Time.deltaTime);

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
