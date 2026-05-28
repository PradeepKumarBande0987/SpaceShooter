using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]
    private float _speed = 5f;

    private float horizontalInput;
    private float verticalInput;

    private float _horizontalBoundary = 11.3f;
    private float _verticalBoundary = 6.25f;
    private float _verticalMinBoundary = -4.2f;

    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;

    private int _lives = 3;

    [SerializeField]
    private GameObject laserPrefabs;

    private SpawnManager _spawnManager;

    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private bool _isTripleShotActive = false; 

    [SerializeField]
    private bool _isShieldActive = false;

    [SerializeField]
    private GameObject _shieldVisualizer;

    [SerializeField]
    private GameObject _thrusterVisualizer;

    [SerializeField]
    private GameObject _playerHurtRightVisualizer;

    [SerializeField]
    private GameObject _playerHurtLeftVisualizer;

    [SerializeField]
    private int _score = 0;

    private UIManager _uiManager;

    [SerializeField]
    private AudioClip _laserAudioClip;

    private AudioSource _audioSource;

    [System.Obsolete]
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
         _shieldVisualizer.SetActive(false);
         _thrusterVisualizer.SetActive(true);
         _playerHurtRightVisualizer.SetActive(false);
         _playerHurtLeftVisualizer.SetActive(false);
        _spawnManager = FindObjectOfType<SpawnManager>();
        _uiManager = FindObjectOfType<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null) {
            Debug.LogError("Spawn Manager is null!");
        }

        if(_uiManager == null)
        {
            Debug.LogError("UI Manager is null.");
        }

        if(_audioSource == null)
        {
            Debug.LogError("Audio Source in player is null");
        } else
        {
            _audioSource.clip = _laserAudioClip;
        }
    }
    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        Shoot();
    }

    void Shoot() {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire) {
            _canFire = Time.time + _fireRate;
            if (_isTripleShotActive) {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            } else {
                Instantiate(laserPrefabs, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
            }
            _audioSource.Play();
        }
    }

    void CalculateMovement()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        
        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(movement  * _speed * Time.deltaTime);

        if (transform.position.x > _horizontalBoundary) {
            transform.position = new Vector3(_horizontalBoundary, transform.position.y, 0);
        } else if (transform.position.x < -_horizontalBoundary) {
            transform.position = new Vector3(-_horizontalBoundary, transform.position.y, 0);
        }

        if (transform.position.y > _verticalBoundary) {
            transform.position = new Vector3(transform.position.x, _verticalBoundary, 0);
        } else if (transform.position.y < _verticalMinBoundary){
            transform.position = new Vector3(transform.position.x, _verticalMinBoundary, 0);
        }

    }

    public void Damage() {
        if(_isShieldActive == true)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }
        _lives--;

        _uiManager.UpdateLives(_lives);
        if(_lives == 2)
        {
            _playerHurtRightVisualizer.SetActive(true);
        } else if(_lives == 1)
        {
            _playerHurtLeftVisualizer.SetActive(true);
        } else if(_lives < 1) {
            _spawnManager.OnPlayerDeath();
            _thrusterVisualizer.SetActive(false);
            Destroy(this.gameObject);
        }
    }

    public void ActivateSpeedBoost() {
        _speed = 8f;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    private IEnumerator SpeedBoostPowerDownRoutine() {
        yield return new WaitForSeconds(5f);
        _speed = 5f;
    }

    internal void ActivateTripleShot()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    private IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShotActive = false;
    }

    public void activateShield()
    {
        _isShieldActive = true;
         _shieldVisualizer.SetActive(true);
        StartCoroutine(ShieldPowerUpDownRoutine());
    }

    private IEnumerator ShieldPowerUpDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isShieldActive = false;
        _shieldVisualizer.SetActive(false);
    }

    public void updatePlayerScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
