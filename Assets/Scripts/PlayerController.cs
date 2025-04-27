using UnityEngine;

/// <summary>
/// プレイヤーの操作をつかさどるクラス
/// </summary>
public class PlayerController : MonoBehaviour
{
    public void Move(Vector3 direction)
    {
        Vector3 newPosition = transform.position + direction;

        transform.position = newPosition;
    }
}
