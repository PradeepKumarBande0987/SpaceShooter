using System.Collections;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3f;

    private float _horizontalBoundary = 11.3f;
    private float _verticalBoundary = 6.25f;
    
    [SerializeField]
    private float _delayBeforeRespawn = 15f;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Update()
    {
        MovePowerUpDown();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            Player player = collision.GetComponent<Player>();

            if (player != null) {

                switch (gameObject.tag) {
                    case "PowerUp":
                        Debug.Log("Player picked up Triple Shot Power-Up!");
                        player.ActivateTripleShot();
                        break;
                    case "SpeedBoost":
                        Debug.Log("Player picked up Speed Boost Power-Up!");
                        player.ActivateSpeedBoost();
                        break;
                    default:
                        Debug.LogWarning("Unknown power-up type: " + gameObject.tag);
                        break;
                }
            }

            StartCoroutine(RespawnPowerUp());
        }
    }

    private IEnumerator RespawnPowerUp()
    {
        gameObject.SetActive(false);

        yield return new WaitForSeconds(_delayBeforeRespawn);

        transform.position = new Vector3(Random.Range(-_horizontalBoundary, _horizontalBoundary), _verticalBoundary, 0);
        gameObject.SetActive(true);
    }


    void MovePowerUpDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -_verticalBoundary) {
            Vector3 spawnPosition = new Vector3(
                Random.Range(-_horizontalBoundary, _horizontalBoundary),
                _verticalBoundary,
                0
            );

            transform.position = spawnPosition;
        }
    }
}
