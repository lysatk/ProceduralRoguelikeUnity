using Assets._Scripts.Utilities;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    public void DealDamage(float dmg, List<ConditionBase> list)
    {
        var unit = gameObject.GetComponentInParent<UnitBase>();
        unit.TakeDamage(dmg, list);
    }
}