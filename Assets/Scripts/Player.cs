using System;
using System.Collections;
using UnityEngine;

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
    private bool _isSpeedBoostActive = false;

    [SerializeField]
    private GameObject _speedBoostPrefab;

    [System.Obsolete]
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = FindObjectOfType<SpawnManager>();

        if (_spawnManager == null) {
            Debug.LogError("Spawn Manager is null!");
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
        }
    }

    void CalculateMovement()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (_isSpeedBoostActive) {
            _speed = 8f;
        }
        
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
        _lives--;
        if (_lives < 1) {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void ActivateSpeedBoost() {
        _isSpeedBoostActive = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    private IEnumerator SpeedBoostPowerDownRoutine() {
        yield return new WaitForSeconds(5f);
        _isSpeedBoostActive = false;
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
}
