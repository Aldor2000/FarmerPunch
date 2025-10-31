using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;

    //MOVEMENT VARIABLES
    [SerializeField] public float detectRadius = 5f; // La IA leerá este valor
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float stopDistance = 1f;
    [SerializeField] float patrolRadius = 5f;

    //ATTACK VARIABLES
    [SerializeField] public float attackRadius = 1f; // La IA leerá este valor
    [SerializeField] float attackCooldown = 1f;
    [SerializeField] float attackDamage = 25f;
    private float lastAttackTime = 0f;

    bool isDead = false;
    [SerializeField] float health = 100f;


    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
    }

    public void MoveTo(Vector2 targetPos)
    {
        Vector2 currentPos = rb.position;
        Vector2 direction = (targetPos - currentPos).normalized;

        float distanceToTarget = Vector2.Distance(currentPos, targetPos);

        rb.linearVelocity = direction * moveSpeed;
        //Debug.Log("Moving toward: " + targetPos + " current: " + rb.position);
        FaceTarget(targetPos);
    }

    public void StopMoving()
    {
        rb.linearVelocity = Vector2.zero;
    }

    public Vector2 GetRandomPosition()
    {
        Vector2 randomOffset = Random.insideUnitCircle * patrolRadius;
        return (Vector2)transform.position + randomOffset;
    }


    public bool CanAttack()
    {
        // Solo revisamos si el tiempo de enfriamiento ha pasado.
        return Time.time >= lastAttackTime + attackCooldown;
    }

    // Llama a Attack() si el cooldown ha terminado.
    public void TryAttack()
    {
        if (CanAttack())
            Attack();
    }

    public void Attack()
    {
        lastAttackTime = Time.time;
        //PLACEHOLDER
        Debug.Log($"{name} attacked Player for {attackDamage} damage!");
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"{name} took {damage} damage. Remaining health: {health}");

        if (health <= 0)
            Die();
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        StopMoving();
        Debug.Log($"{name} has died!");
        Destroy(gameObject);
    }

    public void FaceTarget(Vector2 targetPos)
    {
        Vector3 scale = transform.localScale;
        if (targetPos.x < transform.position.x && scale.x > 0)
            scale.x *= -1;
        else if (targetPos.x > transform.position.x && scale.x < 0)
            scale.x *= -1;

        transform.localScale = scale;
    }

    // El Gizmos es perfecto, ahora usará los valores de este script
    // que la IA también leerá.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}