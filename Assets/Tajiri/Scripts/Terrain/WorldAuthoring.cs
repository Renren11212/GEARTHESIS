using Unity.Rendering;
using Unity.Entities;
using UnityEditor;
using UnityEngine;

public class WorldAuthoring : MonoBehaviour
{
    public Material[] Materials;
    public Mesh[] Meshes;
    public int Seed;
    public float NoiseScale = 4f;
    public float MaxHeight = 3f;

    /// <summary>
    /// シード値を生成するメソッド
    /// </summary>
    public void GenerateSeed()
    {
        Seed = Random.Range(0, 10000);
    }

    public class WorldDataBaker : Baker<WorldAuthoring>
    {
        // 値が変更されるたびに呼び出される(エディットモードでも)
        public override void Bake(WorldAuthoring authoring)
        {
            // このMonoBehaviourクラスからEntityに変換されたものを取得
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new WorldSettings { seed = authoring.Seed, maxHeight = authoring.MaxHeight, noiseScale = authoring.NoiseScale });
            AddComponentObject(entity, new BlockMeshAndMaterial { renderMeshArray = new RenderMeshArray(authoring.Materials, authoring.Meshes) });
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(WorldAuthoring))]
public class ChunkAuthoringEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        WorldAuthoring worldAuthoring = (WorldAuthoring)target;

        if (GUILayout.Button("Generate Seed"))
        {
            worldAuthoring.GenerateSeed();
        }
    }
}
#endif