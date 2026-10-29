using System;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

// Reference: https://www.youtube.com/watch?v=c9kxUvCKhwQ

namespace Jump.Scripts
{
  public class PlayerWithTransform : MonoBehaviour
  {

    [SerializeField] private float fixedJumpForce = 5f;
    [SerializeField] private float jumpVelocity = 0f;
    [SerializeField] private float gravityScale = 1f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] float moveSpeed = 5f;


    public Transform feet;
    public Collider2D feetCollider;
    public bool isGrounded = true;
    ContactFilter2D contactFilter = new ContactFilter2D();
    Collider2D[] results = new Collider2D[1];
    private Vector2 playerMoveDirection = Vector2.zero;

    void Start()
    {
      contactFilter.SetLayerMask(LayerMask.GetMask("Ground"));
    }
    
    void Update()
    {
      transform.Translate(playerMoveDirection * Time.deltaTime * moveSpeed);

      CheckGroundCheckCollision();
      PerformJumpGravityEffect();
    }

    public void Move(InputAction.CallbackContext context)
    {
      Vector2 moveVector = context.ReadValue<Vector2>();
      
      if (moveVector.y != 0) return;

      playerMoveDirection = moveVector;

      Debug.Log($"OnMove: {moveVector}");
    }

    public void Jump(InputAction.CallbackContext context)
    {
      if (context.performed)
      {
        // jumpVelocity = fixedJumpForce;

        // derivation of: vˆ2 = u^2 + 2*a*s but with v as 0, because we want the height when the final velocity is 0
        jumpVelocity = Mathf.Sqrt(jumpHeight * -2f * (Physics2D.gravity.y * gravityScale));
      }
      else if (context.canceled)
      {
        // Key released
      }
    }

    private void PerformJumpGravityEffect()
    {
      if (isGrounded)
      {
        jumpVelocity = 0;
      }
      else
      {
        // Use the analytical method by default. Switch to Euler for approximation if desired.
        PerformJumpGravityAnalytical();
        // PerformJumpGravityEuler(); // Uncomment to use Euler instead
      }
    }

    // --- Euler Integration (Simple Approximation) ---
    // This method updates velocity first, then uses it to move the object.
    // It assumes velocity is constant during the frame, which is only an approximation.
    // For small Time.deltaTime (high FPS), this is usually good enough.
    private void PerformJumpGravityEuler()
    {
      float acceleration = Physics2D.gravity.y * gravityScale;
      jumpVelocity += acceleration * Time.deltaTime;
      float eulerDeltaY = jumpVelocity * Time.deltaTime;
      transform.Translate(Vector3.up * eulerDeltaY);
    }

    // --- Analytical (Physics Equation) ---
    // This method uses the full equation: d = u * t + 0.5 * a * t^2
    // It accounts for the fact that velocity changes during the frame, so it's more accurate,
    // especially for large Time.deltaTime (low FPS, lag spikes).
    private void PerformJumpGravityAnalytical()
    {
      float acceleration = Physics2D.gravity.y * gravityScale;
      float analyticalDeltaY = jumpVelocity * Time.deltaTime + 0.5f * acceleration * Time.deltaTime * Time.deltaTime;
      transform.Translate(Vector3.up * analyticalDeltaY);
      jumpVelocity += acceleration * Time.deltaTime;
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
