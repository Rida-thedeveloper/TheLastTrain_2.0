using UnityEngine;

public class CockroachAI : MonoBehaviour
{
    public float moveSpeed = 0.15f;
    public float moveRadius = 1f;

    public float minWaitTime = 0.5f;
    public float maxWaitTime = 2f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float waitTimer;

    void Start()
    {
        startPosition = transform.position;

        ChooseNewTarget();
    }

    void Update()
    {
        if (waitTimer > 0)
        {
            waitTimer -= Time.deltaTime;
            return;
        }

        Vector3 direction = targetPosition - transform.position;
        direction.y = 0;

        if (direction.magnitude < 0.05f)
        {
            waitTimer = Random.Range(minWaitTime, maxWaitTime);
            ChooseNewTarget();
            return;
        }

        direction.Normalize();

        transform.position += direction * moveSpeed * Time.deltaTime;

        transform.rotation = Quaternion.LookRotation(direction);
    }

    void ChooseNewTarget()
    {
        Vector2 randomPoint = Random.insideUnitCircle * moveRadius;

        targetPosition = new Vector3(
            startPosition.x + randomPoint.x,
            startPosition.y,
            startPosition.z + randomPoint.y
        );
    }
}