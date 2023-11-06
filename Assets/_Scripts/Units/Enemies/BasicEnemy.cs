using System.Collections;
using UnityEngine;
using UnityEngine.AI; // Required for NavMeshAgent

public class BasicEnemy : EnemyBase
{
    [Header("Settings")]
    [SerializeField] private bool attackFromCenter;
    [SerializeField] private float rangeOfAttack = 0.1f;
    [SerializeField] private float rangeOfRest = 2f;
    [SerializeField] private float rangeOfChase = 5f;
    [SerializeField] private float attackCooldown = 5;
    [SerializeField] private Spell spell;

    [Header("State Management")]
    private bool onCooldown = false;
    private float lastAttack = 0;
    private States currentState;

    // Movement and bias variables
    [Header("Bias Variables")]
    [SerializeField] private float attackPhaseDuration = 1f;
    [SerializeField] private float movementBiasDuration = 5f;
    [SerializeField] private float maxAttackRate = 1f;
    [SerializeField] private float minAttackRate = 0.3f;
    private float attackPhaseTimer = 0f;
    private float lastPlayerDamageTime = 0f;
    private float movementBiasTimer = 0f;
    private float currentAttackRate = 0.1f;

    // Resting state variables
    private float restMovementTimer = 0f;
    private const float restMovementDuration = 1f;
    private const float restMovementRange = 2f;
    private Vector3 restTargetPosition;

 
    private enum States
    {
        Idle,
        Moving,
        Attacking,
        Rest,
        Die
    }

    private void Start()
    {
        InitializeComponents();
        ConfigureNavmeshAgent();
        currentState = States.Moving;
    }

    private void InitializeComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameManager.Player.transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }

    private void ConfigureNavmeshAgent()
    {
        navMeshAgent.speed = stats.MovementSpeed;
        navMeshAgent.stoppingDistance = rangeOfAttack;
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
    }

    private void Update()
    {
        HandleState();
    }

    private void HandleState()
    {
        switch (currentState)
        {
            case States.Idle:
                ChangeState(States.Rest);
                break;
            case States.Moving:
                ProcessMovingState();
                break;
            case States.Attacking:
                ProcessAttackingState();
                break;
            case States.Rest:
                ProcessRestingState();
                break;
            case States.Die:
                // Implement die behavior
                break;
        }
    }

    private void ProcessMovingState()
    {
        if (IsPlayerInRange(rangeOfAttack))
        {
            ChangeState(States.Attacking);
            ResetAttackPhase();
        }
        else if (IsPlayerInRange(rangeOfChase))
        {
            Chasing();
        }
        else
        {
            ChangeState(States.Rest);
        }
    }

    private void ProcessAttackingState()
    {
        if (IsPlayerInRange(rangeOfAttack) && !onCooldown && Time.time - lastAttack >= currentAttackRate)
        {
            Attack();
        }
        else if (!IsPlayerInRange(rangeOfAttack))
        {
            ChangeState(States.Moving);
        }

        attackPhaseTimer += Time.deltaTime;
        currentAttackRate = Mathf.Lerp(minAttackRate, maxAttackRate, attackPhaseTimer / attackPhaseDuration);

        if (Time.time - lastPlayerDamageTime >= movementBiasDuration)
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            Move(dirToPlayer);
        }
    }

    private void ProcessRestingState()
    {
        StopAnimation();

        restMovementTimer += Time.deltaTime;

        if (restMovementTimer >= restMovementDuration)
        {
            Vector2 randomOffset = Random.insideUnitCircle * restMovementRange;
            restTargetPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0f);
            restMovementTimer = 0f;
        }

        Move(restTargetPosition);

        if (Time.time - lastAttack >= rangeOfRest)
        {
            ChangeState(States.Moving);
        }
    }

    private bool IsPlayerInRange(float range)
    {
        float distance = Vector3.Distance(transform.position, player.position);
        return distance <= range;
    }

    private void ChangeState(States newState)
    {
        currentState = newState;
    }

    private void ResetAttackPhase()
    {
        attackPhaseTimer = 0f;
    }

    private void Attack()
    {
        if (onCooldown) return;

        onCooldown = true;
        lastAttack = Time.time;
        lastPlayerDamageTime = Time.time;

        Vector3 dirToPlayer = (player.position - transform.position).normalized;

        if (attackFromCenter)
        {
            spell.Attack(transform.position, Quaternion.identity, projectileLayerName, ObjectPool.SpellSource.Enemy);
        }
        else
        {
            float angle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;
            spell.Attack(transform.position + dirToPlayer, Quaternion.AngleAxis(angle, Vector3.forward), projectileLayerName, ObjectPool.SpellSource.Enemy);
        }

        StartCoroutine(ResetCooldownAfterAnimation());
    }

    private IEnumerator ResetCooldownAfterAnimation()
    {
        yield return new WaitForSeconds(attackCooldown);
        onCooldown = false;
    }

    private void Chasing()
    {
        Move(player.position);
    }
}

