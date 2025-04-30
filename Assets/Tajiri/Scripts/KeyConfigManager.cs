using System.Collections.Generic;
using UnityEngine;

public class KeyConfigManager : MonoBehaviour
{
    // デフォルト設定（初期値）
    private static readonly Dictionary<InputActionType, KeyCode> _defaultBindings = new()
    {
        { InputActionType.MOVE_FORWARD,             KeyCode.W },
        { InputActionType.MOVE_BACK,                KeyCode.S },
        { InputActionType.MOVE_RIGHT,               KeyCode.D },
        { InputActionType.MOVE_LEFT,                KeyCode.A },
        { InputActionType.RIGHT_HAND_ATTACK,        KeyCode.Mouse1 },
        { InputActionType.LEFT_HAND_ATTACK,         KeyCode.Mouse0 },
        { InputActionType.RIGHT_SHOULDER_ATTACK,    KeyCode.E },
        { InputActionType.LEFT_SHOULDER_ATTACK,     KeyCode.Q },
        { InputActionType.JUMP,                     KeyCode.Space },
    };

    // 現在のキー割り当て
    private Dictionary<InputActionType, KeyCode> _currentBindings = new();

    private void Awake()
    {
        ResetToDefault();
        //LoadFromJson();
    }

    /// <summary>
    /// キーバインドを変更
    /// </summary>
    public void ChangeKeyBinding(InputActionType actionType, KeyCode newKey)
    {
        if (_currentBindings.ContainsKey(actionType))
        {
            // 重複キーのチェック
            foreach (var binding in _currentBindings)
            {
                if (binding.Value == newKey && binding.Key != actionType)
                {
                    Debug.LogWarning($"Key {newKey} is already assigned to {binding.Key}");
                    return;
                }
            }
            _currentBindings[actionType] = newKey;
        }
    }

    public KeyCode GetKeyForAction(InputActionType actionType)
    {
        return _currentBindings.TryGetValue(actionType, out var key) ? key : KeyCode.None;
    }

    public void ResetToDefault()
    {
        _currentBindings = new Dictionary<InputActionType, KeyCode>(_defaultBindings);
    }
}