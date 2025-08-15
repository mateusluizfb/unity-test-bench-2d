using UnityEngine;
using UnityEngine.InputSystem;

namespace FollowFixedPoint.Scripts
{
    public class Player : MonoBehaviour
    {
        [Header("Player Settings")]
        [SerializeField] Transform target;
        private Vector2 moveDirection = Vector2.zero;
        private float moveSpeed = 5f;

        // Update is called once per frame
        void Update()
        {
            if (target != null)
            {
                FaceTarget();
            }
            // Move the player every frame
            Vector3 movement = new Vector3(moveDirection.x, moveDirection.y, 0) * moveSpeed * Time.deltaTime;
            transform.position += movement;
        }

        // #region PLAYER_MOVEMENT
        public void OnMove(InputValue inputValue)
        {
            moveDirection = inputValue.Get<Vector2>();
        }
        // #endregion

        private void FaceTarget()
        {
            // Ref: https://www.youtube.com/watch?v=s-Ho5hF2Yww&list=PLMj5RSRN1rwp0R01nByvvYUvffoEyStzk

            if (target != null)
            {
                // Euler method, instant:
                Vector3 direction = (target.position - transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));

                // Slerp method, smoother:
                // Vector3 direction = (target.position - transform.position).normalized;
                // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                // Quaternion targetRotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
                // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }
    }
}
