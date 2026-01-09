using MoreMountains.Feedbacks;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float maxSpeed = 50f;
    [SerializeField, Range(0.0f, 1f)] private float maxYAngle = 0.6f;
    [SerializeField] private MMF_Player hitFeedback;
    [SerializeField] private Rigidbody2D rb;

    Vector2 moveDirection;
    Vector2 velocity;

    bool stopBall = false;

    private void OnEnable()
    {
        GameManager.Instance.onPlayerWin += StopBall;
        GameManager.Instance.onPlayerLost += StopBall;
    }

    private void OnDisable()
    {
        GameManager.Instance.onPlayerWin -= StopBall;
        GameManager.Instance.onPlayerLost -= StopBall;
    }

    private void StopBall()
    {
        stopBall = true;
        rb.linearVelocity = Vector3.zero;
        transform.position = Vector3.zero;
    }

    private void Start()
    {
        // Random direction
        RandomizeDirection();
    }

    private void RandomizeDirection()
    {
        float xDir = Random.value < 0.5f ? -1 : 1;
        float yDir = Random.value < 0.5f ? -1 : 1;
        moveDirection = new Vector2(xDir, yDir);
    }

    private void Update()
    {
        if (stopBall) return;

        Vector3 pos = transform.position;

        // Set the velocity
        velocity = moveDirection.normalized * speed;
    }


    private void FixedUpdate()
    {
        if (stopBall) return;

        rb.linearVelocity = velocity;
        //Debug.Log(rb.linearVelocity);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        hitFeedback?.PlayFeedbacks();

        Transform transformCollider = collision.transform;
        
        float ballPosY = transform.position.y;
        float padPosY = transformCollider.position.y;
        float distance = ballPosY - padPosY;

        if (transformCollider.CompareTag("Border"))
        {
            moveDirection.y = collision.contacts[0].normal.y;
        }
        
        if (transformCollider.CompareTag("Player"))
        {
            moveDirection.y = (distance / (transformCollider.localScale.y / 2)) * maxYAngle;
            moveDirection.x = collision.contacts[0].normal.x;
        }

        if (transformCollider.CompareTag("LeftBorder"))
        {
            if (!GameManager.Instance.infinitMod)
            {
                GameManager.Instance.NewPoint(1, false);
                transform.position = Vector3.zero;
                RandomizeDirection();
            }
            moveDirection.y = (distance / (12 / 2)) * maxYAngle;
            moveDirection.x = collision.contacts[0].normal.x;
        }

        if (transformCollider.CompareTag("RightBorder"))
        {
            if (!GameManager.Instance.infinitMod)
            {
                GameManager.Instance.NewPoint(1, true);
                transform.position = Vector3.zero;
                RandomizeDirection();
            }
            moveDirection.y = (distance / (12 / 2)) * maxYAngle;
            moveDirection.x *= - 1;
        }
        
        SpeedUp();
    }

    private void SpeedUp()
    {
        speed *= 1.05f;
        speed = Mathf.Clamp(speed, 0, maxSpeed);
    }
}
