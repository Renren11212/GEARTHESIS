//  RandomMapMaker.cs
//  http://kan-kikuchi.hatenablog.com/entry/PerlinNoise
//
//  Created by kan kikuchi on 2016.3.14.
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ランダムにマップを生成する
/// </summary>
public class RandomMapMaker : MonoBehaviour
{

  //シード
  private float _seedX, _seedZ;

  //マップのサイズ
  [SerializeField]
  [Header("------実行中に変えれない------")]
  private float _width = 50;
  [SerializeField]
  private float _depth = 50;

  //コライダーが必要か
  [SerializeField]
  private bool _needToCollider = false;

  //高さの最大値
  [SerializeField]
  [Header("------実行中に変えられる------")]
  private float _maxHeight = 10;

  //パーリンノイズを使ったマップか
  [SerializeField]
  private bool _isPerlinNoiseMap = true;

  //起伏の激しさ
  [SerializeField]
  private float _relief = 15f;

  //Y座標を滑らかにするか(小数点以下をそのままにする)
  [SerializeField]
  private bool _isSmoothness = false;

  //マップの大きさ
  [SerializeField]
  private float _mapSize = 1f;

  // 生成済みキューブを管理するリスト
  private List<GameObject> _cubes = new List<GameObject>();

  //=================================================================================
  //初期化
  //=================================================================================

  private void Awake()
  {

  }

  public void GenerateTerrain()
  {
#if UNITY_EDITOR
    if (!Application.isPlaying)
    {
      // Undo登録でエディタ操作を元に戻せるように
      Undo.RegisterFullObjectHierarchyUndo(gameObject, "Generate Terrain");
      ClearAllCubes();
    }
#endif
    //マップサイズ設定
    transform.localScale = new Vector3(_mapSize, _mapSize, _mapSize);

    //同じマップにならないようにシード生成
    _seedX = Random.value * 100f;
    _seedZ = Random.value * 100f;

    //キューブ生成
    for (int x = 0; x < _width; x++)
    {
      for (int z = 0; z < _depth; z++)
      {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localPosition = new Vector3(x, 0, z);
        cube.transform.SetParent(transform);

        if (!_needToCollider)
        {
          DestroyImmediate(cube.GetComponent<BoxCollider>());
        }

        SetY(cube);

        // --- 追加: 管理リストに追加 ---
        _cubes.Add(cube);

#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
          Undo.RegisterCreatedObjectUndo(cube, "Create Cube");
        }
#endif
      }
    }
  }

  public void ClearAllCubes()
  {
    // 子オブジェクトをすべて削除
    for (int i = transform.childCount - 1; i >= 0; i--)
    {
      var child = transform.GetChild(i).gameObject;
#if UNITY_EDITOR
      if (!Application.isPlaying)
        Undo.DestroyObjectImmediate(child);
      else
        Destroy(child);
#else
      Destroy(child);
#endif
    }
    _cubes.Clear();
  }

  //インスペクターの値が変更された時
  private void OnValidate()
  {
    //実行中でなければスルー
    if (!Application.isPlaying)
    {
      return;
    }

    //マップの大きさ設定
    transform.localScale = new Vector3(_mapSize, _mapSize, _mapSize);

    //各キューブのY座標変更
    foreach (Transform child in transform)
    {
      SetY(child.gameObject);
    }
  }

  //キューブのY座標を設定する
  private void SetY(GameObject cube)
{
    float y = 0;

    //パーリンノイズを使って高さを決める場合
    if (_isPerlinNoiseMap)
    {
        float xSample = (cube.transform.localPosition.x + _seedX) / _relief;
        float zSample = (cube.transform.localPosition.z + _seedZ) / _relief;

        float noise = Mathf.PerlinNoise(xSample, zSample);

        y = _maxHeight * noise;
    }
    //完全ランダムで高さを決める場合
    else
    {
        y = Random.Range(0, _maxHeight);
    }

    //滑らかに変化しない場合はyを四捨五入
    if (!_isSmoothness)
    {
        y = Mathf.Round(y);
    }

    //位置設定
    cube.transform.localPosition = new Vector3(cube.transform.localPosition.x, y, cube.transform.localPosition.z);

    //高さによって色を段階的に変更
    Color color = Color.black;//岩盤っぽい色

    if (y > _maxHeight * 0.3f)
    {
        ColorUtility.TryParseHtmlString("#019540FF", out color);//草っぽい色
    }
    else if (y > _maxHeight * 0.2f)
    {
        ColorUtility.TryParseHtmlString("#2432ADFF", out color);//水っぽい色
    }
    else if (y > _maxHeight * 0.1f)
    {
        ColorUtility.TryParseHtmlString("#D4500EFF", out color);//マグマっぽい色
    }

#if UNITY_EDITOR
    if (!Application.isPlaying)
    {
        cube.GetComponent<MeshRenderer>().sharedMaterial.color = color;
    }
    else
#endif
    {
        cube.GetComponent<MeshRenderer>().material.color = color;
    }
}

}

#if UNITY_EDITOR

[CustomEditor(typeof(RandomMapMaker))]
public class RandomMapMakerEditor : Editor
{
  public override void OnInspectorGUI()
  {
    base.OnInspectorGUI();

    //生成ボタンを表示
    if (GUILayout.Button("Generate"))
    {
      ((RandomMapMaker)target).GenerateTerrain();
    }

    //削除ボタンを表示
    if (GUILayout.Button("Clear All"))
    {
      ((RandomMapMaker)target).ClearAllCubes();
    }
  }
}
#endif