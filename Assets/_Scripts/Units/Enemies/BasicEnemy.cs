using System.Collections;
using UnityEngine;

/// <summary>
/// State machine and its logic for enemy
/// </summary>
public class BasicEnemy : EnemyBase
{
    [SerializeField]
    private bool attackFromCenter;
    [SerializeField]
    private float rangeOfAttack = 0.1f;
    [SerializeField]
    private float rangeOfRest = 2f;
    [SerializeField]
    private float rangeOfChase = 5f;
    [SerializeField]
    private float attackCooldown = 5;
    [SerializeField]
    private Spell spell;
    bool onCooldown = false;


    private float lastAttack = 0;


    #region biasVariables
    // ????????????????????
    private float attackPhaseTimer = 0f;
    private float attackPhaseDuration = 1f;
    private float attackCooldownRamp = 1.5f;
    private float lastPlayerDamageTime = 0f;
    private float movementBiasTimer = 0f;
    private float movementBiasDuration = 5f;
    private float maxAttackRate = 1f;
    private float minAttackRate = 0.3f;
    private float currentAttackRate = 0.1f;
    #endregion

    private float restMovementTimer = 0f;
    private float restMovementDuration = 1f;
    private float restMovementRange = 2f;
    private Vector3 restTargetPosition;


    private enum States
    {
        Idle,
        Moving,
        Attacking,
        Rest,
        Die
    }

    private States currentState;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameManager.Player.transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentState = States.Moving;
        _anim = GetComponent<Animator>();
        ConfigureNavmeshAgent();
    }
    public void ConfigureNavmeshAgent()
    {
        navMeshAgent.speed = stats.MovementSpeed;
        navMeshAgent.stoppingDistance = rangeOfAttack;
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        navMeshAgent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.LowQualityObstacleAvoidance;
    }



    void Update()
    {
        switch (currentState)
        {
            case States.Idle:
                ChangeState(States.Rest);
                break;

            case States.Moving:
                if (IsPlayerInRange(rangeOfAttack))
                {
                    ChangeState(States.Attacking);
                }
                else if (IsPlayerInRange(rangeOfChase))
                {
                    Chasing();
                }
                else
                {
                    ChangeState(States.Rest);
                }
                break;

            case States.Attacking:
                if (IsPlayerInRange(rangeOfAttack))
                {

                    if (!onCooldown && Time.time - lastAttack >= currentAttackRate)
                    {
                        Attack();
                    }
                }
                else
                {
                    ChangeState(States.Moving);
                }


                attackPhaseTimer += Time.deltaTime;


                currentAttackRate = Mathf.Lerp(minAttackRate, maxAttackRate, attackPhaseTimer / attackPhaseDuration);

                if (Time.time - lastPlayerDamageTime >= movementBiasDuration)
                {
                    Vector3 dirToPlayer = (player.position - transform.position).normalized;
                    Move(transform.position + dirToPlayer);
                }
                break;

            case States.Rest:
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
                break;

            case States.Die:
                //TO DO
                break;
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


    private void Attack()
    {
        if (onCooldown)
            return;

        onCooldown = true;
        lastAttack = Time.time;
        lastPlayerDamageTime = Time.time;
        if (attackFromCenter)
        {
            spell.Attack(transform.position, Quaternion.identity, projectileLayerName,ObjectPool.SpellSource.Enemy);
            return;
        }



        Vector3 dirToPlayer = (player.position - transform.position);
        dirToPlayer.Normalize();
        float angle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;
        spell.Attack(transform.position + dirToPlayer, Quaternion.AngleAxis(angle, Vector3.forward), projectileLayerName, ObjectPool.SpellSource.Enemy);
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

