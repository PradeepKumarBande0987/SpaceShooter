using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    
    [SerializeField]
    private GameObject _powerUpPrefab;

    [SerializeField]
    private GameObject _speedBoostPrefab;

    [SerializeField]
    private GameObject _enemyContainer;

    private float _horizontalBoundary = 11.3f;
    private float _verticalBoundary = 6.25f;

    private float _spawnRate = 5f;

    private bool _stopSpawning = false;

    private GameObject _currentPowerUp;
    private GameObject _currentSpeedBoost;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerUp());
        StartCoroutine(SpawnSpeedBoost());
    }

    private IEnumerator SpawnPowerUp() 
    {
        while (_stopSpawning == false) {

            if(_currentPowerUp != null) {
                Destroy(_currentPowerUp);
            }

            Vector3 spawnPosition = new Vector3(Random.Range(-_horizontalBoundary, _horizontalBoundary), _verticalBoundary, 0);
            _currentPowerUp = Instantiate(_powerUpPrefab, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3f, 7f));
        }
    }
    private IEnumerator SpawnSpeedBoost() 
    {
        while (_stopSpawning == false) {

            if(_currentSpeedBoost != null) {
                Destroy(_currentSpeedBoost);
            }

            Vector3 spawnPosition = new Vector3(Random.Range(-_horizontalBoundary, _horizontalBoundary), _verticalBoundary, 0);
            _currentSpeedBoost = Instantiate(_speedBoostPrefab, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3f, 7f));
        }
    }

    private IEnumerator SpawnEnemy()
    {
        while (_stopSpawning == false) {
            Vector3 spawnPosition = new Vector3(Random.Range(-_horizontalBoundary, _horizontalBoundary), _verticalBoundary, 0);
            GameObject enemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
            enemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_spawnRate);
        }
    }

    public void OnPlayerDeath() {
        _stopSpawning = true;
    }
}
