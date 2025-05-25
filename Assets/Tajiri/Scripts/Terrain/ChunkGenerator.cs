using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;

public partial struct ChunkGenerator : ISystem
{
    // これは非効率だなぁ。そうに決まってる。
    private bool _initialized;

    public void OnCreate(ref SystemState state)
    {
        _initialized = false;
    }

    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.HasSingleton<WorldSettings>()) return;
        if (!_initialized)
        {
            GenerateChunk(new int3(0, 0, 0), ref state);
            Debug.Log("Generated");
            _initialized = true;
        }
    }

    /// <summary>
    /// 指定した位置を中心にチャンクを生成するメソッド
    /// </summary>
    public void GenerateChunk(int3 chunkPos, ref SystemState state)
    {
        EntityManager entityManager = state.EntityManager;
        Entity worldSettingsEntity = SystemAPI.GetSingletonEntity<WorldSettings>();
        WorldSettings worldSettings = SystemAPI.GetComponent<WorldSettings>(worldSettingsEntity);
        BlockMeshAndMaterial blockMeshAndMaterial = SystemAPI.ManagedAPI.GetComponent<BlockMeshAndMaterial>(worldSettingsEntity);

        float noiseScale = worldSettings.noiseScale;
        float maxHeight = worldSettings.maxHeight;

        var desc = new RenderMeshDescription(
        shadowCastingMode: ShadowCastingMode.On,
        receiveShadows: true );

        for (int x = 0; x < 16; x++)
        {
            for (int z = 0; z < 16; z++)
            {
                int worldX = chunkPos.x - 8 + x;
                int worldZ = chunkPos.z - 8 + z;
                float height = 0f;
                float sampleX = (worldX + worldSettings.seedX) / noiseScale;
                float sampleZ = (worldZ + worldSettings.seedZ) / noiseScale;
                height = maxHeight * (noise.cnoise(new float2(sampleX, sampleZ)) * 0.5f + 0.5f); // Mathmaticのノイズ関数を使用 (範囲を正規化)
                height = math.round(height);

                float3 position = new float3(worldX, height, worldZ);

                var cubeEntity = entityManager.CreateEntity();

                RenderMeshUtility.AddComponents(
                    cubeEntity,
                    entityManager,
                    desc,
                    blockMeshAndMaterial.renderMeshArray,
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
    }
}
