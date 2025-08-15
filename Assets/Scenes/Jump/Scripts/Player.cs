using UnityEngine;
using UnityEngine.InputSystem;

namespace Jump.Scripts
{
    public class Player : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 5f;
        [SerializeField] float jumpForce = 5f;
        private Vector2 playerMoveDirection = Vector2.zero;

        void Start()
        {

        }

        void Update()
        {
            Vector3 movement = new Vector3(playerMoveDirection.x, playerMoveDirection.y, 0) * moveSpeed * Time.deltaTime;
            transform.position += movement;
        }

        void OnMove(InputValue inputValue)
        {
            Vector2 inputDirection = inputValue.Get<Vector2>();

            if (inputDirection.y != 0) return;

            playerMoveDirection = new Vector2(inputDirection.x, 0);
        }

        void OnJump(InputValue inputValue)
        {
            if (inputValue.isPressed)
            {
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
    }
}
