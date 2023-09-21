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
//using Stats = Assets._Scripts.Utilities.Stats;

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
     
    

    protected Transform player;
    protected Animator _anim;
    protected SpriteRenderer spriteRenderer;

    [SerializeField]
    private intSO scoreSO;

    protected NavMeshAgent navMeshAgent;

    protected Collider2D hitbox;

    void Awake()
    {
        conditionsBar = gameObject.transform.GetChild(0);
        _conditionUI = conditionsBar.GetComponent<ConditionUI>();

        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
        }
        hitbox=GetComponentInChildren<Collider2D>();
        //if (hitbox == null)
        //{
        //    Debug.Log("Hitbox Null!");
        //}

    }



    /// <summary>
    /// Add Score, play death animation and remove enity form enemies list 
    /// </summary>
    public override void Die()
    {
        hitbox.enabled = false;
        navMeshAgent.enabled = false;

        base.Die();
        scoreSO.Int++;
        _anim.CrossFade("Death", 0, 0);
        _isDead = true;
        GameManager.enemies.Remove(this.gameObject);
        Destroy(gameObject, 0.8f);
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

        return true;
    }



    protected void Move(Vector3 target)
    {
        if (_isDead) { return; }
        // Assuming you have a reference to the NavMeshAgent component
        if (navMeshAgent == null)
        {
            // If the NavMeshAgent component is not assigned, you should handle this error.
            Debug.LogError("NavMeshAgent not assigned.");
            return;
        }

        if (_canMove)
        {
            navMeshAgent.isStopped = false;
            // Set the destination for the NavMeshAgent
      
            navMeshAgent.SetDestination(target);

            // If the NavMeshAgent is close to its destination, stop walking animation
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                StopAnimation();
            }
            else
            {
                _anim.CrossFade("Walk", 0, 0);
            }

        }
        else
        {
            navMeshAgent.isStopped = true;
            StopAnimation();
        }

    }
}
