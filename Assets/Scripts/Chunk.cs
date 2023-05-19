using UnityEngine;

public struct Block
{

    public short id;
    public short localX;
    public short localY;
    public short localZ;

    public Block(short id, short localX, short localY, short localZ)
    {
        this.id = id;
        this.localX = localX;
        this.localY = localY;
        this.localZ = localZ;
    }

    public static bool isOpaqueByID(short blockID)
    {
        if(blockID == 0 || blockID==7) return false;

        return true;
    }

}

public delegate void BlockFunction(Block block, Chunk c, WorldRenderer wr);

public class Chunk
{
    short[,,] blocks;

    public int chunkX;
    public int chunkZ;

    public static short WIDTH = 16;
    public static short HEIGHT = 512;

    public Chunk(int x, int z)
    {
        chunkX = x;
        chunkZ = z;
        blocks = new short[Chunk.WIDTH, Chunk.HEIGHT, Chunk.WIDTH];
    }

    public Chunk(short[,,] chunkData, int x, int z)
    {
        chunkX = x;
        chunkZ = z;
        // Shallow copy.
        blocks = chunkData;
    }

    public short GetBlockID(Vector3 pos)
    {
        int x = (int)pos.x;
        int y = (int)pos.y;
        int z = (int)pos.z;

        if (y >= HEIGHT) return 0;
        if (y < 0) return 1;

        if (x >= WIDTH)
        {
            if (chunkX + 1 >= WorldManager.WORLD_RADIUS) return 1;
            else return WorldManager.world[chunkX + 1, chunkZ].GetBlockID(new Vector3(0, y, z));
        }

        if (z >= WIDTH)
        {
            if (chunkZ + 1 >= WorldManager.WORLD_RADIUS) return 1;
            else return WorldManager.world[chunkX, chunkZ + 1].GetBlockID(new Vector3(x, y, 0));
        }

        if (x < 0)
        {
            if (chunkX - 1 < 0) return 1;
            else return WorldManager.world[chunkX - 1, chunkZ].GetBlockID(new Vector3(WIDTH-1, y, z));
        }

        if (z < 0)
        {
            if (chunkZ - 1 < 0) return 1;
            else return WorldManager.world[chunkX, chunkZ - 1].GetBlockID(new Vector3(x, y, WIDTH-1));
        }

        // Prevent out of bounds
        if (x >= WIDTH || z >= WIDTH) return 1;
        if (x < 0 || y < 0 || z < 0) return 1;

        return blocks[x, y, z];
    }

    public void ForEachBlock(BlockFunction bf, WorldRenderer wr, Chunk c)
    {
        for (short x = 0; x < Chunk.WIDTH; x++)
        {
            for (short z = 0; z < Chunk.WIDTH; z++)
            {
                for (short y = 0; y < Chunk.HEIGHT; y++)
                {
                    bf(
                        new Block(
                            blocks[x, y, z],
                            x,
                            y,
                            z
                        ),
                        c,
                        wr
                    );
                }
            }
        }
    }
}