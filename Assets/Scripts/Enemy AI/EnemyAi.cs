using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyAI : MonoBehaviour
{
    private Enemy enemy;       // referencia a nuestro controlador
    private Transform player;

    private enum State { Idle, Patrol, Chase, Attack, Dead }
    private State currentState;

    [SerializeField] private float idleTime = 2f;
    private float idleTimer;

    private Vector2 patrolTarget;
    // 'repathCooldown' y 'lastPatrolTime' no se usaban,
    // los he quitado para limpiar, pero puedes volver a ponerlos
    // si los necesitas más adelante.


    void Start()
    {
        enemy = GetComponent<Enemy>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        currentState = State.Idle;
        idleTimer = idleTime;
    }

    void FixedUpdate()
    {
        // Si el jugador no existe, no hacemos nada.
        if (player == null)
        {
            // Opcional: podríamos cambiar a estado 'Patrol' o 'Idle'
            // si el jugador desaparece (ej: muere y se destruye).
            // Por ahora, simplemente paramos la lógica.
            enemy.StopMoving();
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
            // Simplemente cambiamos de estado
            SwitchState(State.Patrol);
        }

        TryDetectPlayer();
    }

    private void HandlePatrol()
    {
        enemy.MoveTo(patrolTarget);
        enemy.FaceTarget(patrolTarget);

        if (Vector2.Distance(transform.position, patrolTarget) < 0.5f)
        {
            idleTimer = idleTime;
            SwitchState(State.Idle);
        }

        // Siempre intentamos detectar al jugador
        TryDetectPlayer();
    }

    private void HandleChase()
    {


        float distance = Vector2.Distance(transform.position, player.position);

        // ¡CORRECIÓN! Usamos 'enemy.detectRadius'
        if (distance > enemy.detectRadius + 2f) // "Leash" o correa
        {
            SwitchState(State.Patrol);
            return;
        }

        // ¡CORRECIÓN! Usamos 'enemy.attackRadius'
        if (distance <= enemy.attackRadius)
        {
            SwitchState(State.Attack);
            return;
        }

        // Si no está fuera de rango ni en rango de ataque, seguimos moviéndonos
        enemy.MoveTo(player.position);
        enemy.FaceTarget(player.position);
    }

    private void HandleAttack()
    {


        float distance = Vector2.Distance(transform.position, player.position);

        // ¡CORRECIÓN! Usamos 'enemy.attackRadius'
        if (distance > enemy.attackRadius)
        {
            SwitchState(State.Chase);
            return;
        }

        // Si estamos en rango de ataque, paramos de movernos,
        // miramos al jugador e intentamos atacar.
        enemy.StopMoving();
        enemy.FaceTarget(player.position);
        enemy.TryAttack(); // 'TryAttack' revisará su propio cooldown
    }

    private void TryDetectPlayer()
    {

        float distance = Vector2.Distance(transform.position, player.position);

        // ¡CORRECIÓN! Usamos 'enemy.detectRadius'
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

        // Lógica de entrada al estado
        if (newState == State.Idle)
        {
            enemy.StopMoving();
            idleTimer = idleTime; // Reiniciamos el timer de idle
        }
    }
}
