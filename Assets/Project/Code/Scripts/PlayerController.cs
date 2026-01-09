using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] InputActionReference verticalInput;
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private Rigidbody2D rb;

    float verticalInputValue = 0f;
    Bounds playerBounds;
    float cameraHeight;

    void Start()
    {
        cameraHeight = Camera.main.orthographicSize;
    }

    void Update()
    {
        verticalInputValue = verticalInput.action.ReadValue<float>();
        playerBounds = playerCollider.bounds;
        float playerHalfHeight = playerBounds.extents.y;

        Vector3 newPos = transform.position + Vector3.up * verticalInputValue * moveSpeed * Time.deltaTime;
        newPos.y = Mathf.Clamp(newPos.y, -cameraHeight + playerHalfHeight, cameraHeight - playerHalfHeight);

        transform.position = newPos;
    }
}
