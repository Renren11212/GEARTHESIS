using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine;
using System.IO;

/// <summary>
/// 
/// </summary>
public partial struct ChunkLoader : ISystem
{
	// これは非効率
	private bool _initialized;

	public void OnCreate(ref SystemState state)
	{
		_initialized = false;
	}

	public void OnUpdate(ref SystemState state)
	{
		if (!SystemAPI.HasSingleton<WorldSettings>()) return;
		if (!_initialized)
		{
			GenerateChunkEntity(new int3(0, 0, 0), ref state);
			Debug.Log("Generated");
			_initialized = true;
		}
	}

	/// <summary>
	/// 指定した位置を中心にチャンクを生成するメソッド
	/// </summary>
	public void GenerateChunkEntity(int3 chunkPos, ref SystemState state)
	{
		EntityManager entityManager = state.EntityManager;
		Entity worldSettingsEntity = SystemAPI.GetSingletonEntity<WorldSettings>();
		WorldSettings worldSettings = SystemAPI.GetComponent<WorldSettings>(worldSettingsEntity);

		ChunkData chunkData = new ChunkData();

		chunkData.chunkPosition = chunkPos;
		chunkData.blockData = GenerateChunkDataArray(worldSettings, chunkPos);

		Entity chunk = entityManager.CreateEntity();

		entityManager.AddComponentData(chunk, chunkData);
	}

	// Helper
	private BlobAssetReference<ChunkBlockDataBlob> GenerateChunkDataArray(WorldSettings worldSettings, int3 chunkPos)
	{
		byte chunkSize = worldSettings.chunkSize;
		int blockCount = chunkSize * chunkSize * chunkSize;

		var builder = new BlobBuilder(Allocator.Temp);
		ref ChunkBlockDataBlob root = ref builder.ConstructRoot<ChunkBlockDataBlob>();

		// blockIDs配列の確保(ChunkSizeが16なら4096 = 2^12 = 1.5byte <- 2byteにしたほうが良いかな)
		BlobBuilderArray<byte> blockIDs = builder.Allocate(ref root.blockIDs, blockCount);

		for (int x = 0; x < chunkSize; x++)
		for (int y = 0; y < chunkSize; y++)
		for (int z = 0; z < chunkSize; z++)
		{
			int index = ChunkBlockDataBlob.GetIndex(x, y, z, chunkSize);

			// ここでノイズや高さ判定に応じてBlockIDを決定
			int worldX = chunkPos.x - chunkSize / 2 + x;
			int worldZ = chunkPos.z - chunkSize / 2 + z;
			float sampleX = (worldX + worldSettings.seedX) / worldSettings.noiseScale;
			float sampleZ = (worldZ + worldSettings.seedZ) / worldSettings.noiseScale;
			float height = worldSettings.maxHeight * (noise.cnoise(new float2(sampleX, sampleZ)) * 0.5f + 0.5f);
			height = math.round(height);

			blockIDs[index] = (byte)BlockID.Stone;
		}
		var blobRef = builder.CreateBlobAssetReference<ChunkBlockDataBlob>(Allocator.Persistent);
		builder.Dispose();

		return blobRef;
	}
	
	// ファイルに保存
	public void SaveChunk(int3 chunkPos, ref SystemState state)
	{
		foreach (var (chunkData, entity) in SystemAPI.Query<RefRO<ChunkData>>().WithEntityAccess())
		{
			if (chunkData.ValueRO.chunkPosition.Equals(chunkPos))
			{
				// var blockData = chunkData.ValueRO.blockData.Value;
				string path = GetChunkFilePath(chunkPos);

				using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
				using (var bw = new BinaryWriter(fs))
				{
					// chunkPosition
					bw.Write(chunkPos.x);
					bw.Write(chunkPos.y);
					bw.Write(chunkPos.z);

					// blockIDs
					//int length = chunkData.ValueRO.blockIDs.Length;
					//bw.Write(length);
					//for (int i = 0; i < length; i++)
					//{
						//bw.Write(blockData.blockIDs[i]);
					//}
				}
				break;
			}
		}
	}

    // helper
    private string GetChunkFilePath(int3 chunkPos)
    {
        return Path.Combine(Application.persistentDataPath, $"chunk_{chunkPos.x}_{chunkPos.y}_{chunkPos.z}.bin");
    }

    // Job化最優先
    public void LoadChunk(int3 chunkPos, ref SystemState state)
    {
        string path = GetChunkFilePath(chunkPos);
        if (!File.Exists(path))
        {
            GenerateChunkEntity(chunkPos, ref state);
            return;
        }

        using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        using (var br = new BinaryReader(fs))
        {
            int x = br.ReadInt32();
            int y = br.ReadInt32();
            int z = br.ReadInt32();
            int length = br.ReadInt32();

            // ブロックデータの復元
            var builder = new BlobBuilder(Allocator.Temp);
            ref ChunkBlockDataBlob root = ref builder.ConstructRoot<ChunkBlockDataBlob>();
            var blockIDs = builder.Allocate(ref root.blockIDs, length);
            for (int i = 0; i < length; i++)
            {
                blockIDs[i] = br.ReadByte();
            }
            var blobRef = builder.CreateBlobAssetReference<ChunkBlockDataBlob>(Allocator.Persistent);
            builder.Dispose();

            // Entity生成
            EntityManager entityManager = state.EntityManager;
            Entity chunk = entityManager.CreateEntity();
            entityManager.AddComponentData(chunk, new ChunkData
            {
                chunkPosition = new int3(x, y, z),
                blockData = blobRef
            });
        }
    }
}
