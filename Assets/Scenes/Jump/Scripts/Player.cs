using UnityEngine;
using UnityEngine.InputSystem;

// Reference: https://www.youtube.com/watch?v=c9kxUvCKhwQ

namespace Jump.Scripts
{
    public class Player : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 5f;
        private Vector2 playerMoveDirection = Vector2.zero;

        void Update()
        {
            Vector3 movement = new Vector3(playerMoveDirection.x, playerMoveDirection.y, 0) * moveSpeed * Time.deltaTime;
            transform.position += movement;

            ControlRigidBodyYVelocity();
        }

        void OnMove(InputValue inputValue)
        {
            Vector2 inputDirection = inputValue.Get<Vector2>();
            if (inputDirection.y != 0) return;
            playerMoveDirection = new Vector2(inputDirection.x, 0);
        }

        void OnJump(InputValue inputValue)
        {
            JumpWithRigidBody();
            // JumpWithVector2();
        }

        #region Jump With RigidBody
        private void JumpWithRigidBody()
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // jumpForce based on specific jumpHeight:
                float jumpHeight = 3f;
                float gravity = Physics2D.gravity.y * rb.gravityScale;
                float jumpForce = Mathf.Sqrt(jumpHeight * gravity * -2) * rb.mass;

                // Fixed jumpForce:
                // float jumpForce = 10f;

                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }

        private void ControlRigidBodyYVelocity()
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            
            // Change upwards and downwards movement velocity based direction:
            if (rb.linearVelocityY > 0)
            {
                rb.gravityScale = 1f; // Normal gravity when moving upwards
            }
            else
            {
                rb.gravityScale = 3f; // Increased gravity when moving downwards
            }
        }
        #endregion

        private void JumpWithVector2()
        {
            // TODO
        }
    }
}
