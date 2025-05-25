using UnityEngine;

public interface IGameAction
{
    // デフォルトの入力タイプ
    InputPressType DefaultInputType { get; }
    // ランタイムで変更可能
    InputPressType CurrentInputType { get; set; }
    
    // デフォルトのキー入力
    KeyCode DefaultKeyCode { get; }
    // ランタイムで変更可能
    KeyCode CurrentKeyCode { get; set; }

    bool CanExecute();

    void Execute();
}
