using UnityEngine;

public class KnifeBehavior : MonoBehaviour
{
    Rigidbody2D rb;
    public bool isTimeStopped = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (rb.linearVelocity.sqrMagnitude > 0.01f && !isTimeStopped)
        {
            float angle = (Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg) - 90;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}
