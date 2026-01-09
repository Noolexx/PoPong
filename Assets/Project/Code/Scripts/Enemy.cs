using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform ball;
    [SerializeField] private Collider2D paddleCollider;
    [SerializeField] private Rigidbody2D rb;

    float shootOffsetY = 0f;
    Bounds paddleBounds;
    float cameraHeight;

    void Start()
    {
        cameraHeight = Camera.main.orthographicSize;
        StartCoroutine(RandomShootDirection());
    }

    void Update()
    {
        FollowBallWithOffset();
    }

    private void FollowBallWithOffset()
    {
        paddleBounds = paddleCollider.bounds;
        float halfHeight = paddleBounds.extents.y;

        // Position cible : balle + offset choisi
        float targetY = ball.position.y - shootOffsetY;

        float clampedY = Mathf.Clamp(
            targetY,
            -cameraHeight + halfHeight,
            cameraHeight - halfHeight
        );

        Vector3 targetPosition = new Vector3(
            transform.position.x,
            clampedY,
            transform.position.z
        );

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        Debug.DrawLine(transform.position, targetPosition, Color.blue);
    }

    private IEnumerator RandomShootDirection()
    {
        while (true)
        {
            paddleBounds = paddleCollider.bounds;
            float halfHeight = paddleBounds.extents.y;

            // Offset aléatoire : haut, centre ou bas
            shootOffsetY = Random.Range(-halfHeight + 0.1f, halfHeight - 0.1f);

            yield return new WaitForSeconds(1.5f);
        }
    }
}
