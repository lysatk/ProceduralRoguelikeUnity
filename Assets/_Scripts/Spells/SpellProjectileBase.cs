using Assets._Scripts.Utilities;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets._Scripts.Spells
{
    /// <summary></summary>
    public class SpellProjectileBase : SpellBase
    {
        protected Rigidbody2D rb;

        protected void SpellAwake()
        {
            SetSpellStats();
            rb = GetComponent<Rigidbody2D>();
            rb.velocity = transform.right * speed;

            if (BeforeDestory())
                Destroy(gameObject, destroyTime);
        }

        /// <summary>the last function called before the spell prefab is destroyed</summary>
        protected virtual bool BeforeDestory()
        {
            return true;
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out AttackHandler attack))
            {
                attack.DealDamage(spellDamage, conditions);

                Destroy(gameObject);
            }
        }
   
        protected virtual void ExplosiveDamageCircle()
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), 4.5f);

            foreach (var collider in hitColliders)
            {
                if (collider.TryGetComponent(out AttackHandler unit))
                    unit.DealDamage(spellDamage, new List<ConditionBase>());
            }
        }
     
       protected virtual void Awake()
        {
            SpellAwake();
        }
    }
}