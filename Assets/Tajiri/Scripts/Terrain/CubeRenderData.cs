using System;
using Unity.Rendering;
using Unity.Entities;

[Serializable]
public class CubeRenderData : IComponentData
{
    public RenderMeshArray renderMeshArray;
}
