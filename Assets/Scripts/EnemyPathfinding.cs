using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;

    private Rigidbody2D rb;
    private Vector2 target;
    private bool hasTarget = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!hasTarget)
            return;

        Vector2 direction = (target - rb.position).normalized;

        rb.MovePosition(rb.position + direction * (moveSpeed * Time.fixedDeltaTime));
    }

    /// <summary>
    /// Define uma posição alvo para o inimigo ir até lá.
    /// </summary>
    public void MoveTo(Vector2 targetPosition)
    {
        target = targetPosition;
        hasTarget = true;
    }

    /// <summary>
    /// Para movimento imediatamente.
    /// </summary>
    public void Stop()
    {
        hasTarget = false;
    }
}
