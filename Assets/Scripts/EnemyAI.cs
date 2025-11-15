using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        Roaming
    }

    [Header("Roaming Settings")]
    [SerializeField] private float roamRadius = 3f;
    [SerializeField] private float minWaitTime = 0.8f;
    [SerializeField] private float maxWaitTime = 2f;
    [SerializeField] private float minMoveDistance = 0.5f;

    private State state;
    private EnemyPathfinding enemyPathfinding;
    private Vector2 startPosition;

    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        state = State.Roaming;

        // IMPORTANTE: Converte de Vector3 para Vector2
        startPosition = (Vector2)transform.position;
    }

    private void Start()
    {
        StartCoroutine(RoamingRoutine());
    }

    private IEnumerator RoamingRoutine()
    {
        while (state == State.Roaming)
        {
            Vector2 target = GetRoamingPosition();
            enemyPathfinding.MoveTo(target);

            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
        }
    }

    private Vector2 GetRoamingPosition()
    {
        // Pega uma direção aleatória dentro do círculo
        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        // Evita direções super pequenas (impede tremedeira)
        if (randomDirection.magnitude < 0.1f)
            randomDirection = new Vector2(1, 0);

        // Cria ponto dentro do raio
        Vector2 pos = startPosition + randomDirection * roamRadius;

        // Evita movimentos mínimos
        if (Vector2.Distance((Vector2)transform.position, pos) < minMoveDistance)
            pos = (Vector2)transform.position + randomDirection * minMoveDistance;

        return pos;
    }
}
