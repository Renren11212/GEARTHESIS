using UnityEngine;

[RequireComponent(typeof(EntityController))]
public class MoveLeftAction : MoveBase
{
    public override Vector3 Direction => Vector3.left;
    public override KeyCode DefaultKeyCode => KeyCode.A;
}