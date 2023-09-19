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

    protected override void OnEnable()
    {
        SpellAwake();
        hasPlayedAnimation = false;
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
            ObjectPool.ReturnObject(gameObject);
        }
    }

 
}