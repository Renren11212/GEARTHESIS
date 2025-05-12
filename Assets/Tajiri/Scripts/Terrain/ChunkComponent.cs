using Unity.Entities;
using Unity.Mathematics;

public struct ChunkComponent : IComponentData
{
    // チャンクの位置
    public int3 Position;

    // 表示用のエンティティ
    public Entity MeshEntity;
}