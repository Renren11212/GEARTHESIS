using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Rendering;
using UnityEngine;
using UnityEditorInternal;
using UnityEngine.Rendering;
using System;
using Unity.VisualScripting;
using NUnit.Framework.Constraints;

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

    public void Start()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        var entityManager = world.EntityManager;

        RenderMeshArray renderMeshArray = new RenderMeshArray(materials, meshes);

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
                    shadowCastingMode: ShadowCastingMode.On,
                    receiveShadows: true);

                // レンダリングコンポーネントを追加
                RenderMeshUtility.AddComponents(
                    cubeEntity,
                    entityManager,
                    desc,
                    renderMeshArray,
                    MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0)
                );

                entityManager.SetComponentData(cubeEntity, LocalTransform.FromPosition(new float3(0, 0, 0)));

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
    }

    private static Color GetColor(float height, float maxHeight)
    {
        if (height > maxHeight * 0.3f)
        {
            UnityEngine.ColorUtility.TryParseHtmlString("#019540FF", out var color);
            return color;
        }
        else if (height > maxHeight * 0.2f)
        {
            UnityEngine.ColorUtility.TryParseHtmlString("#2432ADFF", out var color);
            return color;
        }
        else if (height > maxHeight * 0.1f)
        {
            UnityEngine.ColorUtility.TryParseHtmlString("#D4500EFF", out var color);
            return color;
        }
        return Color.black;
    }
}




