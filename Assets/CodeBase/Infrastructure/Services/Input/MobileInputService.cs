using UnityEngine;

namespace CodeBase.Services.Input
{
  public class MobileInputService : InputService
  {
    private bool _hasSentInput;
    public override Vector2 Axis => GetInputIfTouchedOnce();

    private Vector2 GetInputIfTouchedOnce()
    {
      var isTouching = SimpleInput.GetMouseButton(0);
      var currentInput = new Vector2(SimpleInput.GetAxis(Horizontal), SimpleInput.GetAxis(Vertical));

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
  }
}