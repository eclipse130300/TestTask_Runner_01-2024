using UnityEngine;

namespace CodeBase.Services.Input
{
  public class MobileInputService : InputService
  {
    public override Vector2 Axis => new Vector2(SimpleInput.GetAxis(Horizontal), SimpleInput.GetAxis(Vertical));
  }
}