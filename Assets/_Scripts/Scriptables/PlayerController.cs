using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Takes and handles input and movement for a player character
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 0.7f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;

    Vector2 movementInput;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;

    bool canMove = true;


    [SerializeField]
    public GameObject healthBarManagerObj;
    HealthBarManager healthBar;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        healthBar = FindObjectOfType<HealthBarManager>();
        healthBar.SetMaxHealth(100);
        healthBar.SetHealth(50);
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }
    private void FixedUpdate()
    {

        rb.AddForce(  movementInput * moveSpeed);
        if (movementInput.x < 0)
        {
            spriteRenderer.flipX = false;//left
        }
        else if (movementInput.x > 0)
        {
            spriteRenderer.flipX = true;//right
        }
    }




    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }

}
