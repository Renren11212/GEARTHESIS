using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Rendering;
using UnityEngine;
using System;

public class RandomMapAuthoring : MonoBehaviour
{
    public int width = 50;
    public int depth = 50;
    public float maxHeight = 10f;
    public float relief = 15f;
    public bool isSmooth = false;
    public bool isPerlin = true;
    public RenderMeshArray renderMeshArray;

    public class Baker : Baker<RandomMapAuthoring>
    {
        public override void Bake(RandomMapAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new RandomMapGenerator
            {
                width = authoring.width,
                depth = authoring.depth,
                maxHeight = authoring.maxHeight,
                relief = authoring.relief,
                isSmooth = authoring.isSmooth ? 1 : 0,
                isPerlin = authoring.isPerlin ? 1 : 0,
                seedX = UnityEngine.Random.Range(0f, 100f),
                seedZ = UnityEngine.Random.Range(0f, 100f),
            });

            AddComponentObject(entity, new CubeRenderData
            {
                renderMeshArray = authoring.renderMeshArray
            });
        }
    }
}


public struct RandomMapGenerator : IComponentData
{
    public int width, depth;
    public float maxHeight, relief;
    public float seedX, seedZ;
    public int isPerlin, isSmooth;
}

[Serializable]
public class CubeRenderData : IComponentData
{
    public RenderMeshArray renderMeshArray;
}
