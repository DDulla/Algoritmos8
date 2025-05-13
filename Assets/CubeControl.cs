using UnityEngine;
using UnityEngine.InputSystem;

public class CubeControl : MonoBehaviour
{
    [SerializeField] private float speed;

    private Rigidbody myRigidbody;
    private Vector2 direction;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        myRigidbody.velocity = new Vector3(speed * direction.x, myRigidbody.velocity.y, speed * direction.y);
    }
    #region InputActions
    
    public void OnMovement(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
    }
    #endregion
}
