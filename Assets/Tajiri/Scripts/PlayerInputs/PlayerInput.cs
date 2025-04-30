using UnityEngine;
using System.Collections.Generic;

public class PlayerInput : MonoBehaviour
{
    private List<IGameAction> _actions = new();

    public void RegisterAction(IGameAction action)
    {
        _actions.Add(action);
    }

    private void Update()
    {
        foreach (var action in _actions)
        {
            CheckAction(action);
        }
    }

    private void CheckAction(IGameAction action)
    {
        bool inputReceived = action.CurrentInputType switch
        {
            InputPressType.INSTANT => Input.GetKeyDown(action.CurrentKeyCode),
            InputPressType.CONTINUOUS => Input.GetKey(action.CurrentKeyCode),
            _ => false
        };

        if (inputReceived && action.CanExecute())
        {
            action.Execute();
        }
    }
}
