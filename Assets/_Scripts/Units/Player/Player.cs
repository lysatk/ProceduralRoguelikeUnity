using Assets._Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{

    private Rigidbody2D _rigidbody;

    public override void Die()
    {
        throw new System.NotImplementedException();
    }

    public override void SetStats(Stats stats)
    {
        throw new System.NotImplementedException();
    }

    public override bool TryMove(Vector2 direction)
    {
        throw new System.NotImplementedException();
    }

    void Awake() 
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    private void OnAnimatorMove()
    {
        
    }
}
