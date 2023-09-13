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

            if (BeforeDestroy())
                Destroy(gameObject, destroyTime);
        }

        /// <summary>the last function called before the spell prefab is destroyed</summary>
        protected virtual bool BeforeDestroy()
        {
            return true;
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out AttackHandler attack))
            {
                attack.DealDamage(DMG, conditions);

                Destroy(gameObject);
            }
        }
    }
}