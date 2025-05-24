using Unity.Entities;

public struct RandomMapGenerator : IComponentData
{
    public int width, depth;
    public float maxHeight, relief;
    public float seedX, seedZ;
    public int isPerlin, isSmooth;
}

