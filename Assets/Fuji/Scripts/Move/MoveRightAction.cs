using UnityEngine;

[RequireComponent(typeof(EntityController))]
public class MoveRightAction : MoveBase
{
    public override InputPressType DefaultInputType => InputPressType.CONTINUOUS;
    public override Vector3 Direction => Vector3.right;
    public override KeyCode DefaultKeyCode => KeyCode.D;
}