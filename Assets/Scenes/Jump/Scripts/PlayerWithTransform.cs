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

    public Transform feet;
    public Collider2D feetCollider;
    public bool isGrounded = true;
    ContactFilter2D contactFilter = new ContactFilter2D();
    Collider2D[] results = new Collider2D[1];

    void Start()
    {
      contactFilter.SetLayerMask(LayerMask.GetMask("Ground"));
    }

    void Update()
    {
      CheckGroundCheckCollision();
      PerformJumpGravityEffect();
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
        jumpVelocity += Physics2D.gravity.y * gravityScale * Time.deltaTime; // v = u + at
        transform.Translate(Vector3.up * jumpVelocity * Time.deltaTime); // d = u * t
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
