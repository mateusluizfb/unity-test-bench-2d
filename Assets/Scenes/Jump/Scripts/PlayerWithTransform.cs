using System;
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

    void Update()
    {
      PerformJumpGravityScale();
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
      // Simple jump logic using transform
      transform.Translate(Vector3.up * jumpVelocity * Time.deltaTime);
      // Apply gravity or other movement logic here if needed
      jumpVelocity -= 9.81f * Time.deltaTime; // Simple gravity effect
      if (transform.position.y < 0) // Reset position if below ground level
      {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        jumpVelocity = 0; // Reset velocity when touching the ground
      }
    }

    private void PerformJumpGravityScale()
    {
      jumpVelocity += Physics2D.gravity.y * gravityScale * Time.deltaTime;
      transform.Translate(Vector3.up * jumpVelocity * Time.deltaTime);
    }
  }
}
