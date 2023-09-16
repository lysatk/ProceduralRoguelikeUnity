using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : EnemyBase
{
    public float detectionRange = 5f;
    public float attackRate = 2f;

    private NavMeshAgent agent;
    private float attackTimer;
    private bool isFlustered;

    private NavMeshPath currentPath; // Stores the calculated path
    private int currentPathIndex; // Index of the current path node

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        attackTimer = attackRate;
        isFlustered = true;

        currentPath = new NavMeshPath();
        currentPathIndex = 0;
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            if (isFlustered)
            {
                // Calculate a path to the player's position
                agent.CalculatePath(player.position, currentPath);

                // If the path is valid and has nodes, follow it
                if (currentPath.status == NavMeshPathStatus.PathComplete && currentPath.corners.Length > 1)
                {
                    currentPathIndex = 0;
                    agent.SetPath(currentPath);
                }

                // Decrease attack timer
                attackTimer -= Time.deltaTime;

                if (attackTimer <= 0f)
                {
                    // Enemy attacks (you can add shooting logic here)
                    Attack();

                    // Reset the attack timer
                    attackTimer = attackRate;
                }
            }
            else
            {
                // Implement more complex AI logic using context steering
                // For example, steer away from obstacles or towards cover
                // You can use the path information in currentPath to make decisions
            }
        }
        else
        {
            // Enemy is out of range, stop the NavMeshAgent
            agent.isStopped = true;

            // You can add idle animations or behaviors here
        }
    }

    private void Attack()
    {
        // Implement enemy attack logic here
        // This can include shooting at the player or any other actions
    }
}
