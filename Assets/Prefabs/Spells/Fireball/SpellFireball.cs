using Assets._Scripts.Spells;
using UnityEngine;

public class SpellFireball : SpellProjectileBase
{
    protected void Awake()
    {
        SpellAwake();
    }

    //void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.TryGetComponent(out AttackHandler attack))
    //    {
    //        attack.DealDamage(DMG, conditions);

    //        Destroy(gameObject);
    //    }
    //}
}