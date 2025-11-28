using UnityEngine;

public class KnifeWeapon : IWeapon
{
    GameObject knifePrefab;
    Transform throwPoint;
    float knifeSpeed;

    public KnifeWeapon(GameObject prefab, Transform point, float speed)
    {
        knifePrefab = prefab;
        throwPoint = point;
        knifeSpeed = speed;
    }

    public void Use(Transform playerTransform)
    {
        Vector2 targetDirection = Vector2.right; // default direction
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(
            UnityEngine.InputSystem.Mouse.current.position.ReadValue()
        );
        targetDirection = (mousePos - throwPoint.position).normalized;

        GameObject knife = Object.Instantiate(knifePrefab, throwPoint.position, Quaternion.identity);
        
        Rigidbody2D rb = knife.GetComponent<Rigidbody2D>();

        float angle = (Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg) - 90;
        knife.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        rb.linearVelocity = targetDirection * knifeSpeed;
    }
}
