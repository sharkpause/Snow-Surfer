using UnityEngine;

public class EnemyParascope : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponentInParent<EnemyMovement>().HandleWallCollide(collision);
    }
}
