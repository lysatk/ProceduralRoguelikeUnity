using Assets._Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public int hitPointsCurrent;
    public int hitPointsMax;
    public int speed;

    public abstract void SetStats(Stats stats);

  //  public abstract void TakeDamage(float dmg, List<ConditionBase> conditions);

    public abstract bool TryMove(Vector2 direction);

    public abstract void Die();
}
