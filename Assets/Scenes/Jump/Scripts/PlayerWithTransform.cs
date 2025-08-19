using UnityEngine;
using UnityEngine.InputSystem;

// Reference: https://www.youtube.com/watch?v=c9kxUvCKhwQ

namespace Jump.Scripts
{
  public class PlayerWithTransform : MonoBehaviour
  {
    void Update()
    {
    }

    public void Move(InputAction.CallbackContext context)
    {
      Debug.Log($"OnMove: {context.ReadValue<Vector2>()}");
    }

    public void Jump(InputAction.CallbackContext context)
    {
      Debug.Log($"OnJump: {context.ReadValue<float>()}");
    }

  }
}
