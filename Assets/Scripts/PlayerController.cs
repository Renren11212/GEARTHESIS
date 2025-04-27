using UnityEngine;

/// <summary>
/// プレイヤーの操作をつかさどるクラス
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField, Tooltip("移動制限エリア")]
    private Area area;

    private bool isAreaSelected = true;

    private void Awake()
    {
        if (area == null)
        {
            isAreaSelected = false;
        }
    }

    public void Move(Vector3 direction)
    {
        Vector3 newPosition = transform.position + direction;

        // 移動制限エリアが指定されているならそれに従う
        if (area.IsContain(newPosition) && isAreaSelected)
        {
            transform.position = newPosition;
        }

        else
        {
            transform.position = newPosition;
        }
    }
}
