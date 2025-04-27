using UnityEngine;

/// <summary>
/// プレイヤーの操作をつかさどるクラス
/// </summary>
public class PlayerController : MonoBehaviour
{
    // 縦軸のレイヤー
    private int _currentLayer;

    /// <summary>
    /// 移動メソッド
    /// </summary>
    public void Move(Vector3 direction)
    {
        Vector3 newPosition = transform.position + direction;

        transform.position = newPosition;
    }
}
