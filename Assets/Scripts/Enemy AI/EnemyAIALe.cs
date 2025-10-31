using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(EnemyAle))]
public class EnemyAIALe : MonoBehaviour
{
    private EnemyAle enemy;       // referencia a nuestro controlador
    private Transform player;

    private enum State { Idle, Patrol, Chase, Attack, Dead }
    private State currentState;

    [SerializeField] private float idleTime = 2f;
    private float idleTimer;

    private Vector2 patrolTarget;

    void Start()
    {
        enemy = GetComponent<EnemyAle>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        currentState = State.Idle;
        idleTimer = idleTime;
    }

    void FixedUpdate()
    {
        if (player == null)
        {
            // Si el jugador desaparece, volvemos a Idle
            SwitchState(State.Idle);
            return;
        }

        switch (currentState)
        {
            case State.Idle:
                HandleIdle();
                break;
            case State.Patrol:
                HandlePatrol();
                break;
            case State.Chase:
                HandleChase();
                break;
            case State.Attack:
                HandleAttack();
                break;
            case State.Dead:
                break;
        }
    }

    private void HandleIdle()
    {
        idleTimer -= Time.fixedDeltaTime;

        if (idleTimer <= 0f)
        {
            // Cambiamos a patrullar (SwitchState le dará un destino)
            SwitchState(State.Patrol);
        }

        TryDetectPlayer();
    }

    private void HandlePatrol()
    {
        //DEBUG
        float distance = Vector2.Distance(transform.position, patrolTarget);
        Debug.Log($"[HandlePatrol] Moviendo a {patrolTarget}. Distancia restante: {distance}");

        enemy.MoveTo(patrolTarget);
        

        if (Vector2.Distance(transform.position, patrolTarget) < 0.1f)
        {
            SwitchState(State.Idle);
        }

        TryDetectPlayer();
    }

    private void HandleChase()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > enemy.detectRadius + 2f) // "Leash"
        {
            SwitchState(State.Patrol); // Ahora esto funcionará
            return;
        }

        if (distance <= enemy.attackRadius)
        {
            SwitchState(State.Attack);
            return;
        }

        enemy.MoveTo(player.position);
        enemy.FaceTarget(player.position);
    }

    private void HandleAttack()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > enemy.attackRadius)
        {
            SwitchState(State.Chase);
            return;
        }

        enemy.StopMoving();
        enemy.FaceTarget(player.position);
        enemy.TryAttack();
    }

    private void TryDetectPlayer()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < enemy.detectRadius)
        {
            SwitchState(State.Chase);
        }
    }

    private void SwitchState(State newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        Debug.Log("Switching to state: " + newState);

        // --- Lógica de Entrada a los Estados ---
        if (newState == State.Idle)
        {
            enemy.StopMoving();
            idleTimer = idleTime;
        }
        else if (newState == State.Patrol)
        {
            // ¡ESTA ES LA CORRECCIÓN!
            // Cada vez que entremos a Patrullar,
            // obtenemos un nuevo punto aleatorio.
            patrolTarget = enemy.GetRandomPosition();

            Debug.Log($"[SwitchState] Nuevo patrolTarget elegido: {patrolTarget}. Posición actual: {transform.position}");
        }
    }
}