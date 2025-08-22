using System;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

// Reference: https://www.youtube.com/watch?v=c9kxUvCKhwQ

/*
  Questions for later:
  - Why multiply by Time.deltaTime twice when using PerformJumpGravityScale?
  - Why does the sqrt formula work for jumpVelocity and it keeps it in a fixed height?
*/


namespace Jump.Scripts
{
  public class PlayerWithTransform : MonoBehaviour
  {

    [SerializeField] private float fixedJumpForce = 5f;
    [SerializeField] private float jumpVelocity = 0f;
    [SerializeField] private float gravityScale = 1f;
    [SerializeField] private float jumpHeight = 2f;

    public Transform feet;
    public Collider2D feetCollider;
    public bool isGrounded;
    ContactFilter2D contactFilter = new ContactFilter2D();
    Collider2D[] results = new Collider2D[1];

    void Start()
    {
      contactFilter.SetLayerMask(LayerMask.GetMask("Ground"));
    }

    void Update()
    {
      CheckGroundCheckCollision();
      PerformJumpGravityScale();
      // PerformJumpSimpleGravityEffect();
    }

    public void Move(InputAction.CallbackContext context)
    {
      Debug.Log($"OnMove: {context.ReadValue<Vector2>()}");
    }

    public void Jump(InputAction.CallbackContext context)
    {
      if (context.performed)
      {
        // jumpVelocity = fixedJumpForce;
        jumpVelocity = Mathf.Sqrt(jumpHeight * -2f * (Physics2D.gravity.y * gravityScale));
      }
      else if (context.canceled)
      {
        // Key released
      }
    }

    private void PerformJumpSimpleGravityEffect()
    {
      transform.Translate(Vector3.up * jumpVelocity * Time.deltaTime);
      jumpVelocity -= 9.81f * Time.deltaTime; // Simple gravity effect

      if (isGrounded)
      {
        jumpVelocity = 0; // Reset jump velocity when grounded
      }
    }

    private void PerformJumpGravityScale()
    {
      if (isGrounded)
      {
        jumpVelocity = 0;
      }
      else
      {
        jumpVelocity += Physics2D.gravity.y * gravityScale * Time.deltaTime;
        transform.Translate(Vector3.up * jumpVelocity * Time.deltaTime);
      }
    }

    private void CheckGroundCheckCollision()
    {
      // Collision with overlap box is needed when there's no RigidBody attached:
      if (Physics2D.OverlapBox(feet.position, feet.localScale, 0f, contactFilter, results) > 0 && jumpVelocity <= 0)
      {
        isGrounded = true;
      }
      else
      {
        isGrounded = false;
      }
    }
  }
}
