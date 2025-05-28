using System.Globalization;
using UnityEngine;

public interface IGameAction
{
    string ActionName { get; }
    // デフォルトの入力タイプ
    InputPressType DefaultInputType { get; }
    // ランタイムで変更可能
    InputPressType CurrentInputType { get; }
    
    // デフォルトのキー入力
    KeyCode DefaultKeyCode { get; }
    // ランタイムで変更可能
    KeyCode CurrentKeyCode { get; }

    bool CanExecute();

    void SetKeyConfig(KeyCode _);

    void SetInputType(InputPressType _);

    void Execute();
}
