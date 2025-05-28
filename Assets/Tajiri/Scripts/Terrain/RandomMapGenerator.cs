using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using System;
using UnityEditor.Build.Pipeline.Tasks;

public class RandomMapGenerator : MonoBehaviour
{
    [SerializeField] private int chunkCountX = 6;
    [SerializeField] private int chunkCountZ = 6;
    [SerializeField] private int chunkSize = 16;
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

        for (int cx = 0; cx < chunkCountX; cx++)
        {
            for (int cz = 0; cz < chunkCountZ; cz++)
            {
                // チャンクデータ生成
                //ChunkData chunk = new ChunkData();
                //chunk.chunkPos = new int3(cx * chunkSize, 0, cz * chunkSize);
                //.blocks = new byte[chunkSize, 1, chunkSize];

                // チャンク内のブロック生成
                for (int x = 0; x < chunkSize; x++)
                {
                    for (int z = 0; z < chunkSize; z++)
                    {
                        int worldX = cx * chunkSize + x;
                        int worldZ = cz * chunkSize + z;

                        float height = 0f;
                        if (isPerlin)
                        {
                            float sampleX = (worldX + seedX) / relief;
                            float sampleZ = (worldZ + seedZ) / relief;
                            height = maxHeight * Mathf.PerlinNoise(sampleX, sampleZ);
                        }
                        else
                        {
                            height = UnityEngine.Random.Range(0f, maxHeight);
                        }
                        if (!isSmooth) height = math.round(height);

                        //chunk.blocks[x, 0, z] = (byte)math.clamp(height, 0, 255);

                        float3 position = new float3(worldX, height, worldZ);

                        var cubeEntity = entityManager.CreateEntity();
                        var desc = new RenderMeshDescription(
                            shadowCastingMode: ShadowCastingMode.Off,
                            receiveShadows: false);

                        RenderMeshUtility.AddComponents(
                            cubeEntity,
                            entityManager,
                            desc,
                            renderMeshArray,
                            MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0)
                        );

                        entityManager.AddComponentData(cubeEntity, LocalTransform.FromPosition(position));
                        entityManager.AddComponentData(cubeEntity, new WorldRenderBounds
                        {
                            Value = new AABB
                            {
                                Center = position,
                                Extents = new float3(0.5f, 0.5f, 0.5f)
                            }
                        });
                    }
                }
                // 必要ならchunkをリストや辞書で管理
            }
        }
        Debug.Log("地形が生成されました");
    }
}




