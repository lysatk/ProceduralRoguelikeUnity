using Assets._Scripts.Spells;
using UnityEngine;

public class SpellPoison : SpellProjectileBase
{
    protected void Awake()
    {
        SetSpellStats();
        SpellAwake();
    }


}