using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;

public partial struct ChunkGenerator : ISystem
{
    private bool initialized;

    public void OnCreate(ref SystemState state)
    {
        initialized = false;
    }

    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.HasSingleton<WorldSettings>()) return;
        if (!initialized)
        {
            GenerateChunk(new int2(0, 0), ref state);
            Debug.Log("Generated");
            initialized = true;
        }
    }

    [BurstCompile]
    public void GenerateChunk(int2 chunkPos, ref SystemState state)
    {
        World world = World.DefaultGameObjectInjectionWorld;
        EntityManager entityManager = world.EntityManager;

        Entity worldSettingsEntity = SystemAPI.GetSingletonEntity<WorldSettings>();
        WorldSettings worldSettings = SystemAPI.GetComponent<WorldSettings>(worldSettingsEntity);
        BlockMeshAndMaterial blockMeshAndMaterial = SystemAPI.ManagedAPI.GetComponent<BlockMeshAndMaterial>(worldSettingsEntity);

        for (int x = 0; x < 16; x++)
        {
            for (int z = 0; z < 16; z++)
            {
                int worldX = 16 + x;
                int worldZ = 16 + z;
                float height = 0f;
                float sampleX = (chunkPos.x + x + (worldSettings.Seed * 0.5f)) / 16;
                float sampleZ = (chunkPos.y + z + worldSettings.Seed) / 16;
                height = 3 * Mathf.PerlinNoise(sampleX, sampleZ);
                height = math.round(height);

                float3 position = new float3(sampleX, height, sampleZ);

                var cubeEntity = entityManager.CreateEntity();
                var desc = new RenderMeshDescription(
                    shadowCastingMode: ShadowCastingMode.Off,
                    receiveShadows: false);

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
