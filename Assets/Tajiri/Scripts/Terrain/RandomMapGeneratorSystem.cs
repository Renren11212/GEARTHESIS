using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Rendering;
using UnityEngine;
using UnityEditorInternal;
using UnityEngine.Rendering;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct RandomMapGeneratorSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<RandomMapGenerator>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var generatorEntity = SystemAPI.GetSingletonEntity<RandomMapGenerator>();
        var generator = SystemAPI.GetComponent<RandomMapGenerator>(generatorEntity);
        var renderData = SystemAPI.ManagedAPI.GetComponent<CubeRenderData>(generatorEntity);

        var entityManager = state.EntityManager;

        for (int x = 0; x < generator.width; x++)
        {
            for (int z = 0; z < generator.depth; z++)
            {
                float height = 0f;
                if (generator.isPerlin == 1)
                {
                    float sampleX = (x + generator.seedX) / generator.relief;
                    float sampleZ = (z + generator.seedZ) / generator.relief;
                    height = generator.maxHeight * Mathf.PerlinNoise(sampleX, sampleZ);
                }
                else
                {
                    height = UnityEngine.Random.Range(0f, generator.maxHeight);
                }

                if (generator.isSmooth == 0)
                    height = math.round(height);

                float3 position = new float3(x, height, z);

                var cubeEntity = entityManager.CreateEntity();

                entityManager.AddComponentData(cubeEntity, LocalTransform.FromPosition(position));
                entityManager.AddComponentData(cubeEntity, new WorldRenderBounds    // 描画最適化
                {
                    Value = new AABB
                    {
                        Center = position,
                        Extents = new float3(0.5f, 0.5f, 0.5f)
                    }
                });

                // RenderMeshArray をセット（shared component）
                entityManager.AddSharedComponentManaged(cubeEntity, renderData.renderMeshArray);
            }
        }

        entityManager.DestroyEntity(generatorEntity);
    }

    private static Color GetColor(float height, float maxHeight)
    {
        if (height > maxHeight * 0.3f)
        {
            ColorUtility.TryParseHtmlString("#019540FF", out var color);
            return color;
        }
        else if (height > maxHeight * 0.2f)
        {
            ColorUtility.TryParseHtmlString("#2432ADFF", out var color);
            return color;
        }
        else if (height > maxHeight * 0.1f)
        {
            ColorUtility.TryParseHtmlString("#D4500EFF", out var color);
            return color;
        }
        return Color.black;
    }
}




