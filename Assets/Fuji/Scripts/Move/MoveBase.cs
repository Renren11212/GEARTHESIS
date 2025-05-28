using UnityEngine;

[RequireComponent(typeof(EntityController))]
public abstract class MoveBase : MonoBehaviour, IGameAction
{
    [SerializeField]
    private PlayerMovement _playerMovement;

    public abstract Vector3 Direction { get; }

    public virtual InputPressType DefaultInputType => InputPressType.CONTINUOUS;
    public InputPressType CurrentInputType { get; set; }

    public abstract KeyCode DefaultKeyCode { get; }
    public KeyCode CurrentKeyCode { get; set; }

    public abstract string ActionName { get; }

    public virtual bool CanExecute() => _isEnabled;
    private bool _isEnabled = true;

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
        //Direction
    }

    public virtual void SetKeyConfig(KeyCode _)
    {

    }

    public virtual void SetInputType(InputPressType _)
    {

    }
}