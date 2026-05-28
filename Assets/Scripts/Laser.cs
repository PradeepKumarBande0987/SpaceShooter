using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private bool _isEnemyLaser = false;

    [SerializeField]
    private float _speed = 8f;

    [SerializeField]
    private float _enemyLaserSpeed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_isEnemyLaser == false)
        {
            MoveUp();
        } else
        {
            MoveDown();
        }
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _enemyLaserSpeed * Time.deltaTime);

        if (transform.position.y < -4.2f) {

            if (transform.parent != null) {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > 6.25f) {

            if (transform.parent != null) {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    public void ActiveEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && _isEnemyLaser == true)
        {
            Player player = collision.GetComponent<Player>();
            if(player != null)
            {
                player.Damage();
            }
        }
    }
}
