using NUnit.Framework.Constraints;
using UnityEngine;

[RequireComponent(typeof(EntityController))]
public class TestMoveAction : MonoBehaviour, IGameAction
{
    [SerializeField]
    public string ActionName => "TestMove";

    public InputPressType DefaultInputType => InputPressType.CONTINUOUS;
    public InputPressType CurrentInputType { get; private set; }
    public KeyCode DefaultKeyCode => KeyCode.W;
    public KeyCode CurrentKeyCode { get; private set; }
    public bool CanExecute() => true;

    private EntityController _controller;

    [SerializeField]
    private float _speed;

    // 初期化
    private void Start()
    {
        _controller = GetComponent<EntityController>();
    }

    public void SetKeyConfig(KeyCode newKeyCode)
    {
        CurrentKeyCode = newKeyCode;
    }

    public void SetInputType(InputPressType newPressType)
    {
        CurrentInputType = newPressType;
    }
    
    public void Execute()
    {
        _controller.Move(new(0f, 0f, Time.deltaTime * _speed));
    }
}
