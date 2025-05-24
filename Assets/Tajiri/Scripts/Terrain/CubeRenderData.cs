using System;
using Unity.Rendering;
using Unity.Entities;
using UnityEditor.SpeedTree.Importer;
using UnityEngine;

[Serializable]
public class CubeRenderData : IComponentData
{
    public RenderMeshArray renderMeshArray;
}
