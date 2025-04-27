using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

/// <summary>
/// 多角形エリア生成コンポーネント
/// </summary>
public class Area : MonoBehaviour
{
    [SerializeField]
    private int _layer = 0;

    [SerializeField]
    private List<Vector2> _points = new List<Vector2>();

    public void GenerateSquare()
    {
        // Clear the previous points
        _points.Clear();
        Vector2 center = new Vector2(transform.position.x, transform.position.y);

        // Create a square with 4 points
        _points.Add(new Vector2(center.x - 1, center.y - 1));
        _points.Add(new Vector2(center.x + 1, center.y - 1));   
        _points.Add(new Vector2(center.x + 1, center.y + 1));
        _points.Add(new Vector2(center.x - 1, center.y + 1));
    }

    public bool IsContain(Vector2 point)
    {
        bool inside = false;

        int count = _points.Count();

        for (int i = 0; i < count; i++)
        {
            Vector2 a = _points[i];
            Vector2 b = _points[(i + 1) % count];

            if (((a.y > point.y) != (b.y > point.y)) && (point.x < (b.x - a.x) * (point.y - a.y) / (b.y - a.y + Mathf.Epsilon) + a.x))
            {
                inside = !inside;
            }
        }

        return inside;
    }

    private void OnDrawGizmos()
    {
        if (_points == null || _points.Count < 2)
            return;

        Gizmos.color = Color.green;
    
        for (int i = 0; i < _points.Count; i++)
        {
            int j = (i + 1) % _points.Count;
            Vector3 current = new Vector3(_points[i].x, _points[i].y, _layer);
            Vector3 next = new Vector3(_points[j].x, _points[j].y, _layer);
            
            Vector3 currentWorld = transform.TransformPoint(current);
            Vector3 nextWorld = transform.TransformPoint(next);
            
            Gizmos.DrawLine(currentWorld, nextWorld);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Area))]
public class MovableAreaEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Area area = (Area)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Generate Square"))
        {
            area.GenerateSquare();
        }
    }
}
#endif


