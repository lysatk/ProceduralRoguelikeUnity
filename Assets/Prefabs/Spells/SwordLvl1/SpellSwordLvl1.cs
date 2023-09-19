using Assets._Scripts.Spells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSwordLvl1 : SpellProjectileBase
{
    [SerializeField]
    float radius;
    [SerializeField]
    Animator animator;
    protected void OnEnable()
    {
        SetSpellStats();
        Animation();
    }

    void Animation()
    {
        animator.speed *= 2.5f;
        animator.enabled = true; // Enable the Animator
        StartCoroutine(WaitForAnimationToEnd());
        ExplosiveDamageCircle(radius);
    }


    IEnumerator WaitForAnimationToEnd()
    {
        // Wait until the animation finishes playing
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return null;
        }

        // Animation has finished playing, destroy the object
        ObjectPool.ReturnObject(gameObject);
    }

    private void ExplosiveDamage()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), radius);

        foreach (var collider in hitColliders)
        {
            if (collider.TryGetComponent(out AttackHandler unit))
                unit.DealDamage(spellDamage, conditions);
        }
    }
}