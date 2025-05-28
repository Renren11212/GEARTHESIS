using UnityEngine;

[RequireComponent(typeof(EntityController))]
public abstract class MoveBase : MonoBehaviour, IGameAction
{
    [SerializeField]
    private PlayerInput _input;

    [SerializeField]
    protected float _speed = 5f;

    protected EntityController _controller;

    public abstract Vector3 Direction { get; }

    public abstract InputPressType DefaultInputType { get; }
    public InputPressType CurrentInputType { get; set; }

    public abstract KeyCode DefaultKeyCode { get; }
    public KeyCode CurrentKeyCode { get; set; }

    public virtual bool CanExecute() => _isEnabled;
    private bool _isEnabled = true;

    protected virtual void Awake()
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

    protected virtual void Update()
    {
        // Cキーで有効・無効切り替え
        if (Input.GetKeyDown(KeyCode.C))
        {
            _isEnabled = !_isEnabled;
        }
    }

    public virtual void Execute()
    {
        if (!_isEnabled) return;
        _controller.Move(Direction * _speed * Time.deltaTime);
    }
}