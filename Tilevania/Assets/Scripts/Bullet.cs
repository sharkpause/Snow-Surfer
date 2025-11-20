using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 10f;
    PlayerMovement playerMovement;
    Rigidbody2D rigidbody;

    float xSpeed;
    float bulletDestroyDelay = 0.05f;

    bool isShooting = false;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        xSpeed = playerMovement.transform.localScale.x * bulletSpeed;
    }

    void Update()
    {
        rigidbody.linearVelocity = new Vector2(xSpeed, 0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Enemy"))
        {
            Destroy(collision.collider.gameObject);
        }

        Destroy(gameObject, bulletDestroyDelay);
    }
}
