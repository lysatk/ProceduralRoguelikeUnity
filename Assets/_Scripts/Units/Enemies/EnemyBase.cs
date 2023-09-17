using Assets._Scripts.Utilities;
using Assets.Resources.SOs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.Impl;
using Random = UnityEngine.Random;
using Stats = Assets._Scripts.Utilities.Stats;

/// <summary>
/// Base logic for enemy
/// </summary>
public abstract class EnemyBase : UnitBase
{
    #region MovementParam
    /// <summary>
    /// Filter for collisons detection
    /// </summary>
    public ContactFilter2D movementFilter;
    /// <summary>
    /// Offset for collisons detection
    /// </summary>
    public float collisionOffset = 0.05f;

    protected Rigidbody2D rb;
    List<RaycastHit2D> castCollisions = new();
  // bool _canMove = true;
    protected Vector2 heading;
    protected float[] wages = new float[8];

    #endregion

    #region PatrolParam
    protected Vector2 PatrolPoint;
    protected float PatrolRadius;
    private Vector2 randomDestination;
    private float lastPatrol = 0;
    #endregion

    #region SensesParam
    /// <summary>
    /// Distance of player detection
    /// </summary>
    public float seeDistance = 10f;
    protected float coneAngle = 45f;
    protected float coneDistance = 5f;
    protected float coneDirection = 180;
    #endregion

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    protected AiData aiData = new();
    [SerializeField]
    private ContextSolver contextSolver = new();

    protected Transform player;
    protected Animator _anim;
    protected SpriteRenderer spriteRenderer;

    [SerializeField]
    private intSO scoreSO;

    NavMeshAgent navMeshAgent;

    void Awake()
    {
        conditionsBar = gameObject.transform.GetChild(0);

        _conditionUI = conditionsBar.GetComponent<ConditionUI>();

        // Check if the GameObject already has a NavMeshAgent component
        navMeshAgent = GetComponent<NavMeshAgent>();

        // If the NavMeshAgent component doesn't exist, add it
        if (navMeshAgent == null)
        {
            navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
            // You can configure the NavMeshAgent's properties here if needed
            
            // Set the destination or other NavMeshAgent settings as needed
         
        }
       //  navMeshAgent.speed = stats.MovementSpeed; // Example: Set the speed
      //  navMeshAgent.speed = 1.0f;
        navMeshAgent.stoppingDistance = 5.0f; // Example: Set the stopping distance
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }
    /// <summary>
    /// Add Score, play death animation and remove enity form enemies list 
    /// </summary>
    public override void Die()
    {
        navMeshAgent.isStopped=true;
        base.Die();
        scoreSO.Int++;
        _anim.CrossFade("Death", 0, 0);
        _isDead = true;
        GameManager.enemies.Remove(this.gameObject);
        Destroy(gameObject, 3f);
    }
    protected void StopAnimation()
    {
        _anim.CrossFade("Idle", 0, 0);
    }

    /// <summary>
    /// Check if the next position is valid for movement
    /// </summary>
    /// <param name="direction">Direction to be checked</param>
    /// <returns></returns>
    public override bool TryMove(Vector2 direction)
    {
        //if (!_canMove)
        //{
        //    if(_isDead)
        //        return false;
        //    StopAnimation();
        //    return false;
        //}

        //_anim.CrossFade("Walk", 0, 0);
        //direction.Normalize();

        //if (direction != Vector2.zero)
        //{
        //    // Check for potential collisions
        //    int count = rb.Cast(
        //        direction, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
        //        movementFilter, // The settings that determine where a collision can occur on such as layers to collide with
        //        castCollisions, // List of collisions to store the found collisions into after the Cast is finished
        //        stats.MovementSpeed * Time.fixedDeltaTime + collisionOffset); // The amount to cast equal to the movement plus an offset

        //    if (count == 0)
        //    {
        //        //Debug.Log(direction);
        //        return true;
        //    }
        //    return false;
        //}
        //else
        //{
        //    _anim.CrossFade("Idle", 0, 0);
        //}


        return false;
    }

  

    protected void Move()
    {
        // Assuming you have a reference to the NavMeshAgent component
        if (navMeshAgent == null)
        {
            // If the NavMeshAgent component is not assigned, you should handle this error.
            Debug.LogError("NavMeshAgent not assigned.");
            return;
        }

        // Set the destination for the NavMeshAgent
        Vector3 newDestination = player.transform.position; // Replace this with your logic for choosing a destination.
        navMeshAgent.SetDestination(newDestination);

        // If the NavMeshAgent is close to its destination, stop walking animation
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            _anim.CrossFade("Idle", 0, 0);
        }
        else
        {
            _anim.CrossFade("Walk", 0, 0);
        }

        // You don't need to manually handle flipping the sprite, NavMeshAgent should handle it automatically.
    }
}
