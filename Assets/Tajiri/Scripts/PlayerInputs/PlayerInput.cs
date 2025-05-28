using System.IO;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;

/// <summary>
/// プレイヤーの入力を監視するクラス
/// </summary>
public class PlayerInput : MonoBehaviour
{
    private List<IGameAction> _actions = new();
    private ActionConfigList _configList = new();

    // build時 dataPath -> persistentDataPathに変更
    private string ConfigPath => Path.Combine(Application.dataPath, "StreamingAssets/ActionConfig.json");

    public void ChangeKeyConfig(IGameAction action, KeyCode newKeyCode)
    {
        int index = _actions.IndexOf(action);

        if (index == -1)
        {
            Debug.LogWarning($"PlayerInput : \"{action.ActionName}\"はインスタンス化されていません.");
        }
        else
        {
            _actions[index].SetKeyConfig(newKeyCode);
        }
    }

    private void Awake()
    {
        _actions = new List<IGameAction>();

        // シーン内のIGameActionをすべて取得
        foreach (var mb in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
        {
            if (mb is IGameAction action)
            {
                _actions.Add(action);
            }
        }

        LoadOrCreateConfig();

        // 設定をIGameActionに反映
        foreach (var action in _actions)
        {
            var config = _configList.configs.FirstOrDefault(c => c.ActionName == action.ActionName);
            if (config != null)
            {
                action.SetInputType(action.DefaultInputType);
                action.SetKeyConfig(action.DefaultKeyCode);
            }
            else
            {
                // デフォルト値で新規追加
                var newConfig = new ActionConfig
                {
                    ActionName = action.ActionName,
                    InputPressType = action.DefaultInputType,
                    KeyCode = action.DefaultKeyCode
                };
                _configList.configs.Add(newConfig);
                action.SetInputType(action.DefaultInputType);
                action.SetKeyConfig(action.DefaultKeyCode);
            }
        }

        SaveConfig();
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

    private void LoadOrCreateConfig()
    {
        if (File.Exists(ConfigPath))
        {
            var json = File.ReadAllText(ConfigPath);
            _configList = JsonUtility.FromJson<ActionConfigList>(json);
            if (_configList == null) _configList = new ActionConfigList();
        }
        else
        {
            _configList = new ActionConfigList();
        }
    }

    private void SaveConfig()
    {
        // ディレクトリがなければ作成
        var dir = Path.GetDirectoryName(ConfigPath);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        var json = JsonUtility.ToJson(_configList, true);
        File.WriteAllText(ConfigPath, json);
    }
}

[System.Serializable]
public class ActionConfig
{
    public string ActionName;
    public InputPressType InputPressType;
    public KeyCode KeyCode;
}

[System.Serializable]
public class ActionConfigList
{
    public List<ActionConfig> configs = new();
}
