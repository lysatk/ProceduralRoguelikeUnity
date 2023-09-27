using Assets._Scripts.Spells;
using System.Collections;
using UnityEngine;

public class SpellBossSlash : SpellProjectileBase
{
    [SerializeField]
    private float radius;
    [SerializeField]
    private Animator animator;

    protected void Awake()
    {
        SpellAwake();
        Animation();
    }

    void Animation()
    {
        animator.speed *= 2.5f;
        animator.enabled = true; // Enable the Animator
        StartCoroutine(WaitForAnimationToEnd());
        ExplosiveDamage();
    }

    IEnumerator WaitForAnimationToEnd()
    {
        // Wait until the animation finishes playing
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return null;
        }

        Destroy(gameObject);
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