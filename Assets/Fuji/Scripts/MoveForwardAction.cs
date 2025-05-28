using UnityEngine;

[RequireComponent(typeof(EntityController))]
public class MoveForwardAction : MoveBase
{
    public override Vector3 Direction => Vector3.forward;
    public override KeyCode DefaultKeyCode => KeyCode.W;
}
