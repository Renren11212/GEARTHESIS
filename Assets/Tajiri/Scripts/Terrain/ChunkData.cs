using Unity.Mathematics;
using Unity.Entities;
using Unity.Rendering;

public struct ChunkData
{
    public int3 chunkPos;
    public byte[,,] blocks;
}

/// <summary>
/// チャンク内のブロックデータ（存在フラグとID）
/// </summary>
public struct ChunkBlockDataBlob
{
    public BlobArray<byte> Exists; // 1:存在, 0:空
    public BlobArray<byte> BlockIDs; // ブロックID
}

public struct ChunkComponent : IComponentData
{
    public int3 ChunkPosition;
    public BlobAssetReference<ChunkBlockDataBlob> BlockData;
}

/// <summary>
/// 世界のシード
/// </summary>
public struct WorldSettings : IComponentData
{
    public int seed;
    public int seedX => seed * 43; // 任意の素数
    public int seedZ => seed * 51;

    public float noiseScale;
    public float maxHeight;
    public byte chunkSize;
}

/// <summary>
/// 使用するブロックのメッシュとマテリアル
/// </summary>
public class BlockMeshAndMaterial : IComponentData
{
    public RenderMeshArray renderMeshArray;
}