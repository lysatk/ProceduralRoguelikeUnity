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
    [Header("Wandering Variables")]
    [SerializeField] private float wanderingRange = 3f;
    [SerializeField] private float wanderingDuration = 2f;
    private float wanderingTimer = 0f;
    private Vector3 wanderingTargetPosition;
    private float attackPhaseTimer = 0f;

    private float currentAttackRate = 0.1f;

    // Resting state variables
    private float restMovementTimer = 0f;
    [SerializeField] private  float restMovementDuration = 1f;
    
    private Vector3 restTargetPosition;

    private bool _aiActive=false;
    private enum States
    {
        Idle,
        Moving,
        Attacking,
        Rest
    }


    private void Start()
    {
        InitializeComponents();
        ConfigureNavmeshAgent();
        
        currentState = States.Idle;
        StartCoroutine(DelayedActivation(1.3f)); 
    }

    private IEnumerator DelayedActivation(float delay)
    {
        yield return new WaitForSeconds(delay);
        _aiActive = true; 
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
        navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.MedQualityObstacleAvoidance;
    }

    private void Update()
    {
        if (_aiActive) 
        HandleState();
    }

    private void HandleState()
    {
        switch (currentState)
        {
            case States.Idle:
                ProcessIdleState();
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
            
        }
    }

    private void ProcessIdleState()
    {
        wanderingTimer += Time.deltaTime;
        if (wanderingTimer >= wanderingDuration)
        {
            wanderingTargetPosition = GetRandomWanderDestination();
            wanderingTimer = 0f;
        }

        Move(wanderingTargetPosition);

        if (IsPlayerInRange(rangeOfChase))
        {
            ChangeState(States.Moving);
        }
    }


    private Vector3 GetRandomWanderDestination()
    {
        Vector2 randomOffset = Random.insideUnitCircle * wanderingRange;
        return transform.position + new Vector3(randomOffset.x, randomOffset.y, 0f);
    }
    private void ProcessMovingState()
    {
        if (IsPlayerInRange(rangeOfAttack))
        {
            ChangeState(States.Attacking);
            attackPhaseTimer = 0f;
        }
        else if (IsPlayerInRange(rangeOfChase))
        {
            Move(player.position);
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
        ChangeState(States.Rest);   
        
    }

    private void ProcessRestingState()
    {
       restMovementTimer += Time.deltaTime;

        if (restMovementTimer >= restMovementDuration)
        {
            restTargetPosition = GetEdgeRestPosition();
            restMovementTimer = 0f;
        }

        Move(restTargetPosition);

        if (IsPlayerInRange(rangeOfChase))
        {
            ChangeState(States.Moving);
        }
        else 
        {
            ChangeState(States.Idle);
        }
    }

    private Vector3 GetEdgeRestPosition()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector3 restPosition = player.position + new Vector3(randomDirection.x, randomDirection.y, 0f) * rangeOfRest;
        return restPosition;
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

  

    private void Attack()
    {
        if (onCooldown) return;

        onCooldown = true;
        lastAttack = Time.time;
      
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

        StartCoroutine(ResetCooldown());
    }

    private IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        onCooldown = false;
    }

    
}

