using Assets._Scripts.Spells;
using Assets._Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDarkMeteor : SpellProjectileBase
{
    [SerializeField]
    private Animator darkMeteorAnimator;
    private bool hasPlayedAnimation = false;

    protected override void Awake()
    {
        SpellAwake();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out AttackHandler unit))
        {
            unit.DealDamage(spellDamage, conditions);

            BeforeDestory();
        }
    }

    protected override bool BeforeDestory()
    {
        StartCoroutine(AnimateTextureChange());
        rb.velocity = Vector2.zero;
        return false;
    }

    private IEnumerator AnimateTextureChange()
    {
        if (!hasPlayedAnimation)
        {
            darkMeteorAnimator.enabled = true; 
            ExplosiveDamageCircle();
            yield return new WaitForSeconds(darkMeteorAnimator.GetCurrentAnimatorStateInfo(0).length);
            darkMeteorAnimator.enabled = false;
            hasPlayedAnimation = true;
            Destroy(gameObject);
        }
    }

    //private void ExplosiveDamage()
    //{
    //    Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), 4.5f);

    //    foreach (var collider in hitColliders)
    //    {
    //        if (collider.TryGetComponent(out AttackHandler unit))
    //            unit.DealDamage(spellDamage, new List<ConditionBase>());
    //    }
    //}
}