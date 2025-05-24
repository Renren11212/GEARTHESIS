using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class RandomMapGenerator : MonoBehaviour
{
    [SerializeField] private int width = 100;
    [SerializeField] private int depth = 100;
    [SerializeField] private float maxHeight = 10;
    [SerializeField] private float relief = 15;
    [SerializeField] private bool isSmooth;
    [SerializeField] private bool isPerlin;
    [SerializeField] private Material[] materials;
    [SerializeField] private Mesh[] meshes;

    private float seedX;
    private float seedZ;

    private void Awake()
    {
        Debug.Log("地形生成を開始します");

        // worldとEntityManagerを取得
        var world = World.DefaultGameObjectInjectionWorld;
        var entityManager = world.EntityManager;

        RenderMeshArray renderMeshArray = new RenderMeshArray(materials, meshes);

        //　シード値を生成
        seedX = UnityEngine.Random.Range(0f, 100f);
        seedZ = UnityEngine.Random.Range(0f, 100f);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                float height = 0f;

                if (isPerlin)
                {
                    float sampleX = (x + seedX) / relief;
                    float sampleZ = (z + seedZ) / relief;
                    height = maxHeight * Mathf.PerlinNoise(sampleX, sampleZ);
                }
                else
                {
                    height = UnityEngine.Random.Range(0f, maxHeight);
                }

                if (!isSmooth) height = math.round(height);

                float3 position = new float3(x, height, z);

                var cubeEntity = entityManager.CreateEntity();

                var desc = new RenderMeshDescription(
                    shadowCastingMode: ShadowCastingMode.Off,
                    receiveShadows: false);

                // レンダリングコンポーネントを追加
                RenderMeshUtility.AddComponents(
                    cubeEntity,
                    entityManager,
                    desc,
                    renderMeshArray,
                    MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0)
                );

                entityManager.AddComponentData(cubeEntity, LocalTransform.FromPosition(position));

                entityManager.AddComponentData(cubeEntity, new WorldRenderBounds    // 描画最適化
                {
                    Value = new AABB
                    {
                        Center = position,
                        Extents = new float3(0.5f, 0.5f, 0.5f)
                    }
                });
            }
        }
        Debug.Log("地形が生成されました");
    }
}




