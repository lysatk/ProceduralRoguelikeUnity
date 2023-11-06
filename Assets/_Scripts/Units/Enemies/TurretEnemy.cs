using System.Collections;
using UnityEngine;

public class TurretEnemy : EnemyBase
{
    [Header("Settings")]
    [SerializeField] private float rangeOfAttack = 10f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private Spell spell;

    private bool onCooldown = false;
    private float lastAttack = 0;

    private enum States
    {
        Idle,
        Attacking,
        Die
    }

    private States currentState;

    void Start()
    {
        player = GameManager.Player.transform;
        currentState = States.Idle;
    }

    void Update()
    {
        HandleState();
    }

    private void HandleState()
    {
        switch (currentState)
        {
            case States.Idle:
                if (IsPlayerInRange(rangeOfAttack))
                {
                    ChangeState(States.Attacking);
                }
                break;
            case States.Attacking:
                ProcessAttackingState();
                break;
            case States.Die:
                // Implement die behavior
                break;
        }
    }

    private void ProcessAttackingState()
    {
        if (!onCooldown && Time.time - lastAttack >= attackCooldown)
        {
            Attack();
        }

        if (!IsPlayerInRange(rangeOfAttack))
        {
            ChangeState(States.Idle);
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
        if (onCooldown) return;

        onCooldown = true;
        lastAttack = Time.time;

        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;
        spell.Attack(transform.position, Quaternion.AngleAxis(angle, Vector3.forward), projectileLayerName, ObjectPool.SpellSource.Enemy);

        StartCoroutine(ResetCooldown());
    }

    private IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        onCooldown = false;
    }
}
