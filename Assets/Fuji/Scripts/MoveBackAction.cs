using UnityEngine;

[RequireComponent(typeof(EntityController))]
public class MoveBackAction : MoveBase
{
    public override Vector3 Direction => Vector3.back;
    public override KeyCode DefaultKeyCode => KeyCode.S;
}