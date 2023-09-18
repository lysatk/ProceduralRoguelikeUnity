using Assets._Scripts.Utilities;
using System.Collections.Generic;
using UnityEngine;


public abstract class SpellBase : MonoBehaviour
{
    [SerializeField]
    protected string Name;
    [SerializeField]
    protected float spellDamage;
    [SerializeField]
    protected List<ConditionBase> conditions;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float destroyTime;

    protected void SetSpellStats()
    {
        var spell = ResourceSystem.Instance.GetExampleSpell(Name);
        spellDamage = spell.spellDamage;
        conditions = spell.conditions;
        speed = spell.speed;
        destroyTime = spell.destroyTime;
    }
}