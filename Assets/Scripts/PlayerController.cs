using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float torqueAmount = 1f;

    InputAction moveAction;
    Rigidbody2D rigidBody;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 moveVector;
        moveVector = moveAction.ReadValue<Vector2>();
        if(moveVector.x < 0)
        {
            rigidBody.AddTorque(torqueAmount);
        } else if(moveVector.x > 0)
        {
            rigidBody.AddTorque(-torqueAmount);
        }
    }
}
