using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    Rigidbody2D rigidbody;

    [SerializeField] float pickUpVelocity = 20f;
    [SerializeField] float fallSpeed = 5f;

    [SerializeField] AudioClip coinPickupSFX;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioSource.PlayClipAtPoint(coinPickupSFX, transform.position);
        gameObject.layer = LayerMask.NameToLayer("Dead");

        float destroyDelay = 1f;

        Destroy(gameObject, destroyDelay);

        rigidbody.gravityScale = fallSpeed;
        rigidbody.linearVelocity = new Vector2(0f, pickUpVelocity);
    }
}