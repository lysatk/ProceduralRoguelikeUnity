using Assets._Scripts.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroUnitBase : UnitBase
{
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;

    Vector2 movementInput;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    BoxCollider2D collider;
    List<RaycastHit2D> castCollisions = new();

    [SerializeField]
    private stringSO mageNameSO;

    [SerializeField]
    Spell PrimarySpell;
    float primaryCooldownCounter = 0f;

    [SerializeField]
    Spell SecondarySpell;
    float secondaryCooldownCounter = 0f;

    [SerializeField]
    Spell QSpell;
    float QCooldownCounter = 0f;

    [SerializeField]
    Spell ESpell;
    float ECooldownCounter = 0f;

    [SerializeField]
    Spell DashSpell;
    float DashCooldownCounter = 0f;
    float _dashMult = 2f;
    float _dashDur = 0.1f;

    [SerializeField]
    private float _invincibilityDurationSeconds = 0.5f;
    //  [SerializeField]
    private float _invincibilityDeltaTime = .1f;

    public bool isInvincible = false;

    StaffRotation spellRotator;

    private Animator _anim;

    protected static string projectileLayerName = "PlayerSpell";
    CooldownUI cooldownUI;

    private bool canChangeMage = false;

    private string nameToChangeMage = null;

    private float baseMovementSpeed; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spellRotator = GetComponentInChildren<StaffRotation>();

        healthBar = FindObjectOfType<HealthBarManager>();
        healthBar.SetMaxHealth(stats.MaxHp);

        cooldownUI = FindObjectOfType<CooldownUI>();

        _anim = GetComponent<Animator>();

        conditionsBar = gameObject.transform.GetChild(0);

        _conditionUI = conditionsBar.GetComponent<ConditionUI>();
        _dashMult = stats.MovementSpeed * 2;
        baseMovementSpeed = stats.MovementSpeed;
    }

    void FixedUpdate()
    {
        if (!_isDead)
            TryMove();
    }

    public void SetNameToChangeMage(string _)
    {
        nameToChangeMage = _;
    }

    public void ChangeMage(string mageName)
    {
        changeRoutine ??= StartCoroutine(WaitAndChange(mageName));
    }

    private IEnumerator WaitAndChange(string mageName)
    {
        yield return new WaitForSeconds(1f);

        this.gameObject.SetActive(false);
        mageNameSO.String = mageName;
        GameManager.Player = UnitManager.Instance.SpawnHero(mageName, this.gameObject.transform.position);
        Destroy(this.gameObject);

        changeRoutine = null;
    }

    #region Movement

    void TryMove()
    {
        if (_canMove)
        {
            Move();
        }
    }

    void Move()
    {
        // If movement input is not 0, try to move
        if (movementInput != Vector2.zero)
        {
            bool success = TryMove(movementInput);
            _anim.CrossFade("Walk", 0, 0);
            //Gliding around walls
            #region Gliding

            if (!success)
            {
                success = TryMove(new Vector2(movementInput.x, 0));
            }

            if (!success)
            {
                _ = TryMove(new Vector2(0, movementInput.y));
            }

            #endregion

        }
        else
        {
            if (stats.CurrentHp > 0)
                _anim.CrossFade("Idle", 0, 0);

        }

        if (movementInput.x < 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (movementInput.x > 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    public override bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            // Check for potential collisions
            int count = collider.Cast(
                direction, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
                movementFilter, // The settings that determine where a collision can occur on such as layers to collide with
                castCollisions, // List of collisions to store the found collisions into after the Cast is finished
                stats.MovementSpeed * Time.fixedDeltaTime + collisionOffset); // The amount to cast equal to the movement plus an offset

            if (count == 0)
            {
                rb.MovePosition(rb.position + stats.MovementSpeed * Time.fixedDeltaTime * direction);
                return true;
            }

            return false;
        }
        return false;
    }
    public bool TryMove(Vector2 direction, float speed)
    {
        if (direction != Vector2.zero)
        {
            // Check for potential collisions
            int count = collider.Cast(
                direction, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
                movementFilter, // The settings that determine where a collision can occur on such as layers to collide with
                castCollisions, // List of collisions to store the found collisions into after the Cast is finished
                speed * Time.fixedDeltaTime + collisionOffset); // The amount to cast equal to the movement plus an offset

            if (count == 0)
            {
                rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * direction);
                return true;
            }

            return false;
        }
        return false;
    }

    #endregion

    public override void TakeDamage(float dmgToTake, List<ConditionBase> conditions)
    {
        if (isInvincible) { return; }
        AudioSystem.Instance.PlayPlayerHitSound();
        base.TakeDamage(dmgToTake, conditions);
        healthBar.SetHealth(stats.CurrentHp);

        iframeRoutine ??= StartCoroutine(BecomeTemporarilyInvincible());
    }

    #region Conditions

    protected void ConditionAffect(List<ConditionBase> conditions)
    {
        if (conditions != null && conditions.Count > 0)
            foreach (ConditionBase condition in conditions)
                Affect(condition);
    }

    protected void Affect(ConditionBase condition)
    {
        switch (condition.Condition)
        {
            case global::Conditions.Burn:

                burnRoutine ??= StartCoroutine(BurnTask(condition));

                break;
            case global::Conditions.Slow:

                slowRoutine ??= StartCoroutine(SlowTask(condition));

                break;
            case global::Conditions.Freeze:

                freezeRoutine ??= StartCoroutine(FreezeTask(condition));

                break;
            case global::Conditions.Poison:

                poisonRoutine ??= StartCoroutine(PoisonTask(condition));

                break;
            case global::Conditions.SpeedUp:

                speedUpRoutine ??= StartCoroutine(SpeedUpTask(condition));

                break;
            case global::Conditions.ArmorUp:

                armorUpRoutine ??= StartCoroutine(ArmorUpTask(condition));

                break;
            case global::Conditions.ArmorDown:

                armorDownRoutine ??= StartCoroutine(ArmorDownTask(condition));

                break;
            case global::Conditions.Haste:

                hasteRoutine ??= StartCoroutine(HasteTask(condition));

                break;
            case global::Conditions.DmgUp:

                dmgUpRoutine ??= StartCoroutine(DmgUpTask(condition));

                break;
            default:
                break;
        }
    }

    override protected IEnumerator BurnTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(0);

        var end = Time.time + condition.AffectTime;

        while (Time.time < end)
        {
            stats.CurrentHp -= Convert.ToInt32(condition.AffectOnTick);

            healthBar.SetHealth(stats.CurrentHp);

            if (stats.CurrentHp <= 0)
                Die();

            yield return new WaitForSeconds(1);
        }

        _conditionUI.RemoveConditionSprite(0);
        burnRoutine = null;
    }

    override protected IEnumerator SlowTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(1);

        var end = Time.time + condition.AffectTime;
        Debug.Log("SlowTask coroutine started");
        float tempSpeed = stats.MovementSpeed; // Store current speed
        stats.MovementSpeed = baseMovementSpeed * (1f - condition.AffectOnTick); // Apply slow to base speed

        stats.MovementSpeed -= stats.MovementSpeed * condition.AffectOnTick;

        while (Time.time < end)
        {
            yield return null;
        }

        _conditionUI.RemoveConditionSprite(1);
        stats.MovementSpeed = tempSpeed; // Revert to previous speed
        Debug.Log("SlowTask coroutine ended");
        slowRoutine = null;
    }

    override protected IEnumerator PoisonTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(2);
        var end = Time.time + condition.AffectTime;

        while (Time.time < end)
        {
            stats.CurrentHp -= Convert.ToInt32(condition.AffectOnTick);

            healthBar.SetHealth(stats.CurrentHp);

            if (stats.CurrentHp <= 0)
                Die();

            yield return new WaitForSeconds(1);
        }

        _conditionUI.RemoveConditionSprite(2);
        poisonRoutine = null;
    }

    override protected IEnumerator FreezeTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(3);

        _canMove = false;

        var end = Time.time + condition.AffectTime;

        while (Time.time < end)
        {
            yield return null;
        }

        _conditionUI.RemoveConditionSprite(3);
        _canMove = true;
        freezeRoutine = null;
    }

    override protected IEnumerator SpeedUpTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(4);

        var end = Time.time + condition.AffectTime;
        var tempMoveSpeed = stats.MovementSpeed;

        stats.MovementSpeed += stats.MovementSpeed * condition.AffectOnTick;

        while (Time.time < end)
        {
            yield return null;
        }

        _conditionUI.RemoveConditionSprite(4);
        stats.MovementSpeed = tempMoveSpeed;
        speedUpRoutine = null;
    }

    override protected IEnumerator ArmorUpTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(5);

        var end = Time.time + condition.AffectTime;
        var tempArmor = stats.Armor;

        stats.Armor += stats.Armor * condition.AffectOnTick;

        while (Time.time < end)
        {
            yield return null;
        }

        stats.Armor = tempArmor;

        _conditionUI.RemoveConditionSprite(5);
        armorUpRoutine = null;
    }

    override protected IEnumerator ArmorDownTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(6);

        var end = Time.time + condition.AffectTime;
        var tempArmorDown = stats.Armor;

        stats.Armor -= stats.Armor * condition.AffectOnTick;

        while (Time.time < end)
        {
            yield return null;
        }

        stats.Armor = tempArmorDown;

        _conditionUI.RemoveConditionSprite(6);
        armorDownRoutine = null;
    }

    override protected IEnumerator HasteTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(7);

        var end = Time.time + condition.AffectTime;
        var tempCooldown = stats.CooldownModifier;

        stats.CooldownModifier += stats.CooldownModifier * condition.AffectOnTick;

        while (Time.time < end)
        {
            yield return null;
        }

        stats.CooldownModifier = tempCooldown;

        _conditionUI.RemoveConditionSprite(7);
        hasteRoutine = null;
    }

    override protected IEnumerator DmgUpTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(8);

        var end = Time.time + condition.AffectTime;
        var tempDmg = stats.DmgModifier;

        stats.DmgModifier += stats.DmgModifier * condition.AffectOnTick;

        while (Time.time < end)
        {
            yield return null;
        }

        stats.DmgModifier = tempDmg;

        _conditionUI.RemoveConditionSprite(8);
        dmgUpRoutine = null;
    }

    #endregion

    #region Input

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnPrimaryAttack()
    {
        if (!_isDead && !GameManager.gamePaused)
            if (Time.time > primaryCooldownCounter)
            {

                CastSpell(PrimarySpell);
                cooldownUI.UpdateCooldown(0, PrimarySpell.cooldown * stats.CooldownModifier);
                primaryCooldownCounter = Time.time + PrimarySpell.cooldown * stats.CooldownModifier;

            }
    }

    void OnSecondaryAttack()
    {
        if (!_isDead && !GameManager.gamePaused)
            if (Time.time > secondaryCooldownCounter)
            {
                cooldownUI.UpdateCooldown(1, SecondarySpell.cooldown * stats.CooldownModifier);
                CastSpell(SecondarySpell);
                secondaryCooldownCounter = Time.time + SecondarySpell.cooldown * stats.CooldownModifier;
            }
    }

    void OnQSpell()
    {
        if (!_isDead && !GameManager.gamePaused)
            if (Time.time > QCooldownCounter)
            {
                CastSpell(QSpell);
                cooldownUI.UpdateCooldown(2, QSpell.cooldown * stats.CooldownModifier);
                QCooldownCounter = Time.time + QSpell.cooldown * stats.CooldownModifier;
            }
    }

    void OnESpell()
    {
        if (!_isDead && !GameManager.gamePaused)
            if (Time.time > ECooldownCounter)
            {
                CastSpell(ESpell);
                cooldownUI.UpdateCooldown(3, ESpell.cooldown * stats.CooldownModifier);
                ECooldownCounter = Time.time + ESpell.cooldown * stats.CooldownModifier;
            }
    }

    void OnDodge()
    {
        Debug.Log("dash");
        if (!_isDead && !GameManager.gamePaused)
        {
            if (Time.time > DashCooldownCounter)
            {
                //stop existing dash coroutines
                if (dashCorutine != null)
                {
                    StopCoroutine(dashCorutine);
                    dashCorutine = null;
                }
                dashCorutine ??= StartCoroutine(Dashing());
                cooldownUI.UpdateCooldown(4, _dashDur + 2 * stats.CooldownModifier);
                DashCooldownCounter = Time.time + _dashDur + 2 * stats.CooldownModifier;

            }

        }
    }

    private IEnumerator Dashing()
    {
        Debug.Log("Dashing coroutine started");
        isInvincible = true;
        float tempSpeed = stats.MovementSpeed; // Store current speed
        stats.MovementSpeed = baseMovementSpeed * _dashMult;

        spriteBlink(0.5f);
        yield return new WaitForSeconds(_dashDur);
        spriteBlink(1);
        Debug.Log("Player is no longer invincible!");

        stats.MovementSpeed = tempSpeed; // Revert to previous speed
        isInvincible = false;
        Debug.Log("Dashing coroutine ended");
        dashCorutine = null;
    }

    void OnStart()
    {
        if (GameManager.Instance.State == GameState.Hub)
            GameManager.Instance.ChangeState(GameState.ChangeLevel);
    }

    void OnRestart()
    {
        if (GameManager.Instance.State == GameState.Starting)
            GameManager.Instance.ChangeState(GameState.Hub);
    }

    void OnInteraction()
    {
        if (nameToChangeMage != null && !GameManager.gamePaused)
        {
            ChangeMage(nameToChangeMage);
        };
    }

    void OnPause()
    {
        Debug.Log("OnPause");
        GameManager.HandlePause();
    }
    #endregion

    void CastSpell(Spell spell)
    {
        if (spellRotator != null)
        {
            if (spell.CastFromHeroeNoStaff)
            {
                spell.caster = collider;
                Debug.Log("From player:"+projectileLayerName);
                spell.Attack(transform.position, spellRotator.StaffFirePoint.transform.rotation, projectileLayerName, ObjectPool.SpellSource.Player);
            }
            else
            {
                spell.caster = collider;
                spell.Attack(spellRotator.StaffFirePoint.transform.position, spellRotator.StaffFirePoint.transform.rotation, projectileLayerName, ObjectPool.SpellSource.Player);
            }
        }
        else
        {
            Debug.LogWarning("SpellRotator is not assigned!");
        }
    }

    public void HideWand()
    {
        spellRotator.gameObject.active = false;
    }
    public void UnHideWand()
    {
        spellRotator.gameObject.active = true;
    }

    public override void Die()
    {
        if (_isDead)
            return;

        base.Die();
        AudioSystem.Instance.PlayPlayerDeathSound();
        _anim.CrossFade("Death", 0, 0);
        HideWand();
        GameManager.Instance.ChangeState(GameState.Lose);
    }

    private void spriteBlink(float alpha)
    {
        spriteRenderer.color = new Color(1, 1, 1, alpha);
    }

    private IEnumerator BecomeTemporarilyInvincible()
    {
        Debug.Log("Player turned invincible!");
        isInvincible = true;

        for (float i = 0; i < _invincibilityDurationSeconds; i += _invincibilityDeltaTime)
        {
            if (spriteRenderer.color.a == 1)
            {
                spriteBlink(0.5f);
            }
            else
            {
                spriteBlink(1f);
            }
            yield return new WaitForSeconds(_invincibilityDeltaTime);
        }
        spriteBlink(1);
        Debug.Log("Player is no longer invincible!");

        isInvincible = false;
        iframeRoutine = null;
    }



}