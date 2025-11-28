using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapons : MonoBehaviour
{
    [SerializeField] GameObject knifePrefab;
    [SerializeField] Transform throwPoint;
    [SerializeField] float knifeSpeed = 15f;

    IWeapon activeWeapon;

    void Start()
    {
        activeWeapon = new KnifeWeapon(knifePrefab, throwPoint, knifeSpeed);
    }

    void Update()
    {
        
    }

    public void OnRangedAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            activeWeapon.Use(transform);
        }
    }
}
