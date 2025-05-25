using Unity.Mathematics;
using Unity.Entities;
using Unity.Rendering;

public struct ChunkData
{
    public int3 chunkPos;
    public byte[,,] blocks;
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

/// <summary>
/// 作成されたチャンクの中心座標
/// </summary>
public struct ChunkPosition : IComponentData
{
    public int3 Value;
}

/// <summary>
/// チャンクのブロックデータ
/// </summary>
public struct ChunkBlocks : IComponentData
{
    public BlobAssetReference<ChunkBlocksBlob> BlockID;
}

public struct ChunkBlocksBlob
{
    public BlobArray<byte> Blocks;
}