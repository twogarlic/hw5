using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public int attackDamage = 1;
    public float detectionRange = 10f;
    public float attackRange = 1.5f;

    private Transform player;
    private Transform baseTarget;
    private Rigidbody rb;

    private enum EnemyState { Idle, Chase, Attack, DestroyBase }
    private EnemyState currentState = EnemyState.Idle;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentState = EnemyState.Idle;
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                LookForPlayer();
                break;
            case EnemyState.Chase:
                ChasePlayer();
                break;
            case EnemyState.Attack:
                AttackTarget();
                break;
            case EnemyState.DestroyBase:
                MoveTowardsBase();
                break;
        }
    }

    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }

    public void SetBase(Transform baseTransform)
    {
        baseTarget = baseTransform;
    }

    void LookForPlayer()
    {
        if (player != null && Vector3.Distance(transform.position, player.position) < detectionRange)
        {
            currentState = EnemyState.Chase;
        }
        else
        {
            currentState = EnemyState.DestroyBase;
        }
    }

    void ChasePlayer()
    {
        if (player == null)
        {
            currentState = EnemyState.DestroyBase;
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < attackRange)
        {
            currentState = EnemyState.Attack;
        }
        else if (distanceToPlayer > detectionRange)
        {
            currentState = EnemyState.DestroyBase;
        }
        else
        {
            MoveTowards(player.position);
        }
    }

    void AttackTarget()
    {
        if (player != null && Vector3.Distance(transform.position, player.position) > attackRange)
        {
            currentState = EnemyState.Chase;
            return;
        }

        if (player == null)
        {
            currentState = EnemyState.DestroyBase;
            return;
        }

        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.TakeDamage(attackDamage);
        }

        Destroy(gameObject);
    }

    void MoveTowardsBase()
    {
        if (baseTarget != null)
        {
            if (Vector3.Distance(transform.position, baseTarget.position) < attackRange)
            {
                currentState = EnemyState.Attack;
            }
            else
            {
                MoveTowards(baseTarget.position);
            }
        }
    }

    void MoveTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Base"))
        {
            BaseController baseController = collision.gameObject.GetComponent<BaseController>();
            if (baseController != null)
            {
                baseController.TakeDamage(attackDamage);
            }

            Destroy(gameObject);
        }
    }
}
