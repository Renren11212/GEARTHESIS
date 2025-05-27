using Unity.Mathematics;
using Unity.Entities;
using Unity.Rendering;

/// <summary>
/// チャンク内のブロックデータ（存在フラグとID）
/// </summary>
public struct ChunkBlockDataBlob
{
    public BlobArray<byte> blockIDs; // ブロックID

    public static int GetIndex(int x, int y, int z,  byte size = 16)
    {
        return x + y * size + z * size * size;
    }

    public void SetBlock(int x, int y, int z, BlockID id, byte size = 16)
    {
        blockIDs[GetIndex(x, y, z, size)] = (byte)id;
    }

    public BlockID GetBlockID(int x, int y, int z, byte size = 16)
    {
        return (BlockID)blockIDs[GetIndex(x, y, z, size)];
    }

    public bool HasBlock(int x, int y, int z, byte size = 16)
    {
        return GetBlockID(x, y, z, size) != BlockID.Air;
    }
}

public enum BlockID
{
    Air,
    Stone,
    Grass,
    Sand
}

public struct ChunkData : IComponentData
{
    public int3 chunkPosition;
    public BlobAssetReference<ChunkBlockDataBlob> blockData;
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