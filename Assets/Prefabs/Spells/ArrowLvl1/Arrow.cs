using Assets._Scripts.Spells;
using UnityEngine;

public class Arrow : SpellProjectileBase
{
    protected void Awake()
    {
        SpellAwake();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out AttackHandler attack))
        {
            attack.DealDamage(spellDamage, conditions);

            Destroy(gameObject);
        }
    }
}