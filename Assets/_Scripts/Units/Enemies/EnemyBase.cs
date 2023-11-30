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
    #endregion


    [SerializeField]
    private LayerMask layerMask;

    private Coroutine flashRoutine; 


    protected Transform player;
    protected Animator _anim;
    protected SpriteRenderer spriteRenderer;

    [SerializeField]
    private intSO scoreSO;

    protected NavMeshAgent navMeshAgent;

    protected Collider2D hitbox;
    protected static string projectileLayerName = "EnemySpell";

  //  protected bool _aiActive = true;


    void Awake()
    {  
        conditionsBar = gameObject.transform.GetChild(0);
        _conditionUI = conditionsBar.GetComponent<ConditionUI>();

        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
        }

        hitbox = GetComponentInChildren<Collider2D>();


        StartCoroutine(StartBehaviorAfterDelay());
    }
    private IEnumerator StartBehaviorAfterDelay()
    {
        yield return new WaitForSeconds(2f);
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

        if (navMeshAgent == null && navMeshAgent.isActiveAndEnabled)
        {
            Debug.LogError("NavMeshAgent not assigned.");

            return;
        }

        if (_canMove)
        {
            navMeshAgent.isStopped = false;


            navMeshAgent.SetDestination(target);


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
    public override void TakeDamage(float dmgToTake, List<ConditionBase> conditions)
    {
        base.TakeDamage(dmgToTake, conditions);
        StartCoroutine(FlashRed());
    }
    public override void TakeDamage(float dmgToTake)
    {
        base.TakeDamage(dmgToTake);
        StartCoroutine(FlashRed());
       spriteRenderer.color = Color.white;
        flashRoutine = null; // Reset the reference when done
    }

    protected virtual void TriggerFlash()
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine); // Stop the existing coroutine if it's running

        flashRoutine = StartCoroutine(FlashRed()); // Start a new coroutine
    }


    private IEnumerator FlashRed()
    {
        Color originalColor = spriteRenderer.color;
        int blinkCount = 5; 
        float blinkDuration = 0.08f; 

        for (int i = 0; i < blinkCount; i++)
        {
            spriteRenderer.color = i % 2 == 0 ? Color.red : Color.white;
            yield return new WaitForSeconds(blinkDuration);
        }

       
        spriteRenderer.color = Color.white;
    }

}
