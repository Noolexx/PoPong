using MoreMountains.Feedbacks;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float initialSpeed = 10f;
    private float currentSpeed = 0f;
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField, Range(0.0f, 1f)] private float maxYAngle = 0.6f;
    [SerializeField] private MMF_Player hitFeedback;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private float cooldown = 1f;
    [SerializeField] CustomTimerEvent timerCooldown;

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
        currentSpeed = initialSpeed;
    }

    public void RandomizeDirection()
    {
        currentSpeed = initialSpeed;

        float xDir = Random.value < 0.5f ? -1 : 1;
        float yDir = Random.value < 0.5f ? -1 : 1;
        moveDirection = new Vector2(xDir, yDir);
    }

    private void Update()
    {
        if (stopBall) return;

        Vector3 pos = transform.position;

        // Set the velocity
        velocity = moveDirection.normalized * currentSpeed;
    }


    private void FixedUpdate()
    {
        if (stopBall) return;

        rb.linearVelocity = velocity;
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
            if (!GameManager.Instance.unableScore)
            {
                GameManager.Instance.NewPoint(1, false);
                NewRound();
            }
            moveDirection.y = (distance / (12 / 2)) * maxYAngle;
            moveDirection.x = collision.contacts[0].normal.x;
        }

        if (transformCollider.CompareTag("RightBorder"))
        {
            if (!GameManager.Instance.unableScore)
            {
                GameManager.Instance.NewPoint(1, true);
                NewRound();
            }
            moveDirection.y = (distance / (12 / 2)) * maxYAngle;
            moveDirection.x *= - 1;
        }
        
        SpeedUp();
    }

    private void NewRound()
    {
        currentSpeed = 0f;
        transform.position = Vector3.zero;
        timerCooldown.StartTimer(cooldown);
    }

    private void SpeedUp()
    {
        currentSpeed *= 1.05f;
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
    }
}
