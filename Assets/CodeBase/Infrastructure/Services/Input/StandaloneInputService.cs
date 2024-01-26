using UnityEngine;

namespace CodeBase.Services.Input
{
  public class StandaloneInputService : InputService
  {
    private bool _hasSentInput;
    public override Vector2 Axis => GetInputIfTouchedOnce();

    private Vector2 GetInputIfTouchedOnce()
    {
      var isTouching = UnityEngine.Input.anyKeyDown || SimpleInput.GetMouseButton(0);
      Vector2 currentInput = SimpleInputAxis();

      if (currentInput == Vector2.zero)
      {
        currentInput = UnityAxis();
      }

      if (isTouching && currentInput.magnitude > 0 && !_hasSentInput)
      {
        _hasSentInput = true;
        return currentInput;
      }

      if (!isTouching)
      {
        _hasSentInput = false;
      }

      return Vector2.zero;
    }
    
    private static Vector2 UnityAxis()
    {
      return new Vector2(UnityEngine.Input.GetAxis(Horizontal), UnityEngine.Input.GetAxis(Vertical));
    }
  }
}