﻿using UnityEngine;

namespace Assets._Scripts.Spells
{
    public class SpellProjectileBase : SpellBase
    {
        protected Rigidbody2D rb;

        protected void MyAwake()
        {
            SetSpellStats();
            rb = GetComponent<Rigidbody2D>();
            rb.velocity = transform.right * speed;

            if (BeforeDestroy())
                Destroy(gameObject, destroyTime);
        }

        public virtual bool BeforeDestroy()
        {
            return true;
        }
    }
}