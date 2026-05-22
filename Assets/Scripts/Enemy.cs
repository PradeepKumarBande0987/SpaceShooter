using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _horizontalBoundary = 11.3f;
    private float _verticalBoundary = 6.25f;
    private float _spawnRate = 4f;

    void Start()
    {
     
    }

    void Update()
    {
        MoveEnemy();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enemy collided with: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Enemy collided with Player. Destroying both.");
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            Destroy(this.gameObject);
        }
        else if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
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