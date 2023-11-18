using Assets._Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets._Scripts.Spells
{
    /// <summary></summary>
    public class SpellProjectileBase : SpellBase
    {
   
        protected Rigidbody2D rb;
        bool _objReturned=false;
        protected void SpellAwake()
        {
            SetSpellStats();
            rb = GetComponent<Rigidbody2D>();
            rb.velocity = transform.right * speed;

            if (BeforeDestory())
            {
                StartCoroutine(WaitForSecondsCoroutine(destroyTime));
            }
        }

        IEnumerator WaitForSecondsCoroutine(float secondsToWait)
        {
            yield return new WaitForSeconds(secondsToWait);
         
            ObjectPool.ReturnObject(gameObject);
        }


        protected virtual bool BeforeDestory()
        {
            return true;
        }

        protected  void OnTriggerEnter2D(Collider2D collision)
        {
         
            if (collision.gameObject.TryGetComponent(out AttackHandler attack) )
            {
                attack.DealDamage(spellDamage, conditions);
            }

            if (!_objReturned)
            {
                ObjectPool.ReturnObject(gameObject);
                _objReturned = true;
            }
            StopAllCoroutines();
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
        protected virtual void ExplosiveDamageCircle(float radius)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), radius);

            foreach (var collider in hitColliders)
            {
                if (collider.TryGetComponent(out AttackHandler unit))
                    unit.DealDamage(spellDamage, new List<ConditionBase>());
            }
        }

        protected virtual void OnEnable()
        {
            SpellAwake();
            _objReturned = false;
           
        }
     
    }
}