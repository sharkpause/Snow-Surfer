using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    Rigidbody2D rigidbody;

    [SerializeField] BoxCollider2D parascopeCollider;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rigidbody.linearVelocity = new Vector2(moveSpeed, 0f);
    }

    public void HandleWallCollide(Collider2D collision)
    {
        if(!(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))) { return; }
        moveSpeed *= -1f;
        FlipSprite();
    }

    void FlipSprite()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(rigidbody.linearVelocity.x)), 1f);
    }
}
