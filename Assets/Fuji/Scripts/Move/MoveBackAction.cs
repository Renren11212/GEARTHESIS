using UnityEngine;

[RequireComponent(typeof(EntityController))]
public class MoveBackAction : MoveBase
{
    public override InputPressType DefaultInputType => InputPressType.CONTINUOUS;
    public override Vector3 Direction => Vector3.back;
    public override KeyCode DefaultKeyCode => KeyCode.S;
}