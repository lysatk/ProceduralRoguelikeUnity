using System.Collections;
using UnityEngine;

public class BossEnemy : EnemyBase
{
    [Header("Boss Settings")]
    [SerializeField] private float[] healthThresholds; // Define thresholds for transitioning to the next phase
    private int currentPhase = 0;
    [SerializeField] private Spell[] attackPatterns;

    private enum States
    {
        Idle,
        Attacking,
        Transitioning, // Animation or behavior when moving to the next phase
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
                // Decide to attack or transition based on certain conditions
                if (ShouldTransitionToNextPhase())
                {
                    ChangeState(States.Transitioning);
                }
                else
                {
                    ChangeState(States.Attacking);
                }
                break;
            case States.Attacking:
                ExecuteRandomAttackPattern();
                // Add logic for when the boss should stop attacking and go back to idle
                break;
            case States.Transitioning:
                ProcessTransitionState();
                break;
            case States.Die:
                // Implement die behavior
                break;
        }
    }

    private bool ShouldTransitionToNextPhase()
    {
        if (currentPhase < healthThresholds.Length && stats.CurrentHp <= healthThresholds[currentPhase])
        {
            currentPhase++;
            return true;
        }
        return false;
    }

    private void ChangeState(States newState)
    {
        currentState = newState;
    }

    private void ExecuteRandomAttackPattern()
    {
        int randomAttackIndex = Random.Range(0, attackPatterns.Length);
        Spell chosenAttack = attackPatterns[randomAttackIndex];

        // Implement logic for chosen attack
        // This can be a method call like chosenAttack.Execute() or similar.
        // You can add more sophisticated logic like making sure the same attack isn't repeated consecutively.
    }

    private void ProcessTransitionState()
    {
        // Add behavior for transitioning, like playing an animation or changing the boss's behavior
        // After transition is complete:
        ChangeState(States.Idle);
    }
}
