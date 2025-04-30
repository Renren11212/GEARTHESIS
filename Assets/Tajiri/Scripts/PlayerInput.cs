using UnityEngine;
using System.Collections.Generic;

public class PlayerInput : MonoBehaviour
{
    // キー設定を管理するマネージャー
    [SerializeField] private KeyConfigManager _keyConfigManager;

    // 各入力アクションのイベント
    private Dictionary<InputActionType, System.Action> ActionEvents = new();

    // 現在の入力デバイス（キーボード/ゲームパッド）
    private IInputDevice _currentInputDevice = new KeyboardInputDevice();

    private void Awake()
    {
        InitializeInputSystem();
    }

    private void Update()
    {
        CheckAllInputs();
    }

    /// <summary>
    /// 入力システムの初期化
    /// </summary>
    public void InitializeInputSystem()
    {
        // 全入力タイプの監視設定
        foreach (InputActionType actionType in System.Enum.GetValues(typeof(InputActionType)))
        {
            ActionEvents[actionType] = null;
        }
    }

    /// <summary>
    /// 全入力のチェック
    /// </summary>
    private void CheckAllInputs()
    {
        foreach (var actionType in ActionEvents.Keys)
        {
            CheckInput(actionType);
        }
    }

    /// <summary>
    /// 特定の入力アクションをチェック
    /// </summary>
    private void CheckInput(InputActionType actionType)
    {
        // キーコンフィグから割り当てキーを取得
        KeyCode key = _keyConfigManager.GetKeyForAction(actionType);

        // そのキーが押されたとき
        if (_currentInputDevice.GetKeyDown(key))
        {
            ActionEvents[actionType]?.Invoke();
            Debug.Log($"Action {actionType} is invoked");
        }
    }

    /// <summary>
    /// 入力アクションを登録（InputActionTypeとメソッドを紐づけるだけ）
    /// </summary>
    public void RegisterAction(InputActionType actionType, System.Action callback)
    {
        if (ActionEvents.ContainsKey(actionType))
        {
            ActionEvents[actionType] += callback;
        }
    }

    /// <summary>
    /// 入力デバイスを切り替え
    /// </summary>
    public void SwitchInputDevice(IInputDevice newDevice)
    {
        _currentInputDevice = newDevice;
        Debug.Log($"Input device switched to: {newDevice.GetType().Name}");
    }
}

// 入力デバイスインターフェース
public interface IInputDevice
{
    bool GetKeyDown(KeyCode key);
    bool GetKey(KeyCode key);
}

// キーボード入力実装
public class KeyboardInputDevice : IInputDevice
{
    public bool GetKeyDown(KeyCode key) => Input.GetKeyDown(key);
    public bool GetKey(KeyCode key) => Input.GetKey(key);
}
