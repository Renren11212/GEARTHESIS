using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.Collections;

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

        byte chunkSize = worldSettings.chunkSize;
        float noiseScale = worldSettings.noiseScale;
        float maxHeight = worldSettings.maxHeight;

        var desc = new RenderMeshDescription(
        shadowCastingMode: ShadowCastingMode.On,
        receiveShadows: true);

        for (int x = 0; x < worldSettings.chunkSize; x++)
        {
            for (int z = 0; z < worldSettings.chunkSize; z++)
            {
                int worldX = chunkPos.x - worldSettings.chunkSize / 2 + x;
                int worldZ = chunkPos.z - worldSettings.chunkSize / 2 + z;
                float height = 0f;
                float sampleX = (worldX + worldSettings.seedX) / noiseScale;
                float sampleZ = (worldZ + worldSettings.seedZ) / noiseScale;
                height = maxHeight * (noise.cnoise(new float2(sampleX, sampleZ)) * 0.5f + 0.5f); // Mathmaticのノイズ関数を使用 (範囲を正規化)
                height = math.round(height);

                float3 position = new float3(worldX, height, worldZ);

                //blockPositions.Add(position);

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

    private BlobAssetReference<ChunkBlockDataBlob> GenerateChunkDataArray(WorldSettings worldSettings, int3 chunkPos)
    {
        byte chunkSize = worldSettings.chunkSize;
        int blockCount = chunkSize * chunkSize * chunkSize;

        var builder = new BlobBuilder(Allocator.Temp);
        ref ChunkBlockDataBlob root = ref builder.ConstructRoot<ChunkBlockDataBlob>();

        // blockIDs配列の確保
        BlobBuilderArray<byte> blockIDs = builder.Allocate(ref root.blockIDs, blockCount);

        // 配列初期化
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    int index = ChunkBlockDataBlob.GetIndex(x, y, z, chunkSize);

                    // ここでノイズや高さ判定に応じてBlockIDを決定
                    int worldX = chunkPos.x - chunkSize / 2 + x;
                    int worldZ = chunkPos.z - chunkSize / 2 + z;
                    float sampleX = (worldX + worldSettings.seedX) / worldSettings.noiseScale;
                    float sampleZ = (worldZ + worldSettings.seedZ) / worldSettings.noiseScale;
                    float height = worldSettings.maxHeight * (noise.cnoise(new float2(sampleX, sampleZ)) * 0.5f + 0.5f);
                    height = math.round(height);

                    blockIDs[index] = (byte)BlockID.Stone;
                }
            }
        }

        var blobRef = builder.CreateBlobAssetReference<ChunkBlockDataBlob>(Allocator.Persistent);
        builder.Dispose();
        return blobRef;
    }

    private void FaceTo()
    {

    }
}
