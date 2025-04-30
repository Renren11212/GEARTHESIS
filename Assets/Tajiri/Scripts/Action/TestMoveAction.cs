using UnityEngine;

[RequireComponent(typeof(EntityController))]
public class TestMoveAction : MonoBehaviour, IGameAction
{
    [SerializeField]
    private PlayerInput _input;

    public InputPressType DefaultInputType => InputPressType.CONTINUOUS;
    public InputPressType CurrentInputType { get; set; }
    public KeyCode DefaultKeyCode => KeyCode.W;
    public KeyCode CurrentKeyCode { get; set; }
    public bool CanExecute() => true;

    private EntityController _controller;

    [SerializeField]
    private float _speed;

    // 初期化
    private void Awake()
    {
        CurrentInputType = DefaultInputType;
        CurrentKeyCode = DefaultKeyCode;

        if (_input == null)
        {
            Debug.LogError("PlayerInputがセットされていません");
            return;
        }

        _controller = GetComponent<EntityController>();
        _input.RegisterAction(this);
    }
    
    public void Execute()
    {
        _controller.Move(new(0f, 0f, Time.deltaTime * _speed));
    }
}
