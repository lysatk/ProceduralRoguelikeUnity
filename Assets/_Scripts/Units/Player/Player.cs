using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{

    private Rigidbody2D _rigidbody;
  void Awake() 
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    private void OnAnimatorMove()
    {
        
    }
}
