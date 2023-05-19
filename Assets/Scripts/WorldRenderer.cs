using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class WorldRenderer : MonoBehaviour
{

    public List<Vector3> verts = new List<Vector3>();
    public List<int> trigs = new List<int>();
    public List<Vector2> uvs = new List<Vector2>();


    public static int ATLAS_WIDTH = 4;
    // With borders
    public static float ATLAS_FULL_BLOCK = 1f / (float)ATLAS_WIDTH;
    // The size of one pixel
    public static float ATLAS_PADDING = (ATLAS_FULL_BLOCK / 6f);
    // The actual center texture by remove two pixels
    public static float ATLAS_BLOCK = (1f / (float)ATLAS_WIDTH) - (2f * ATLAS_PADDING);


    int vertCount = 0;

    public void RenderWorld()
    {
        CreateMeshAndRender();
    }

    public void CreateMeshAndRender()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.Clear();
        mesh.vertices = verts.ToArray();
        mesh.triangles = trigs.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.Optimize();
        mesh.RecalculateNormals();
    }

    public void AddPlant(Vector3 pos, Chunk c)
    {
        AddPlantMesh(pos, c.GetBlockID(pos));
    }

    public void AddPlantMesh(Vector3 pos, short blockID)
    {

        float x = pos.x;
        float y = pos.y;
        float z = pos.z;

        verts.AddRange(new List<Vector3>{
			// Forward
			new Vector3 (x+0, y+0, z+0.5f),
            new Vector3 (x+1, y+0, z+0.5f),
            new Vector3 (x+1, y+1, z+0.5f),
            new Vector3 (x+0, y+1, z+0.5f),

			// Right
			new Vector3 (x+0.5f, y+0, z+0),
            new Vector3 (x+0.5f, y+1, z+0),
            new Vector3 (x+0.5f, y+1, z+1),
            new Vector3 (x+0.5f, y+0, z+1)
        });

        vertCount += 8;

        trigs.AddRange(new List<int>{
			// Foward normals
			vertCount-4-4, vertCount-2-4, vertCount-3-4,
            vertCount-4-4, vertCount-1-4, vertCount-2-4,
            vertCount-4-4, vertCount-3-4, vertCount-2-4,
            vertCount-4-4, vertCount-2-4, vertCount-1-4,
			// Right normals
			vertCount-4, vertCount-2, vertCount-3,
            vertCount-4, vertCount-1, vertCount-2,
            vertCount-4, vertCount-3, vertCount-2,
            vertCount-4, vertCount-2, vertCount-1,
        });

        Vector2 bl = GetAtlasBottomLeft(blockID);

        uvs.AddRange(new List<Vector2>{
            new Vector2(bl.x,bl.y),
            new Vector2(bl.x+ATLAS_BLOCK,bl.y),
            new Vector2(bl.x+ATLAS_BLOCK,bl.y+ATLAS_BLOCK),
            new Vector2(bl.x,bl.y+ATLAS_BLOCK),

            new Vector2(bl.x+ATLAS_BLOCK,bl.y),
            new Vector2(bl.x+ATLAS_BLOCK,bl.y+ATLAS_BLOCK),
            new Vector2(bl.x,bl.y+ATLAS_BLOCK),
            new Vector2(bl.x,bl.y),
        });

    }

    public void AddCube(Vector3 pos, Chunk c)
    {

        //if(c.GetBlockID(pos+Vector3.up) == 0 || c.GetBlockID(pos+Vector3.up) == 7)
        //	AddTopFace(pos, c.GetBlockID(pos));

        //if (c.GetBlockID(pos + Vector3.down) == 0 || c.GetBlockID(pos + Vector3.down) == 7)
        //    AddBottomFace(pos, c.GetBlockID(pos));

        if (c.GetBlockID(pos + Vector3.left) == 0 || c.GetBlockID(pos + Vector3.left) == 7)
            AddRightFace(pos, c.GetBlockID(pos));

        if (c.GetBlockID(pos + Vector3.right) == 0 || c.GetBlockID(pos + Vector3.right) == 7)
            AddLeftFace(pos, c.GetBlockID(pos));

        if (c.GetBlockID(pos + Vector3.back) == 0 || c.GetBlockID(pos + Vector3.back) == 7)
            AddBackFace(pos, c.GetBlockID(pos));

        if (c.GetBlockID(pos + Vector3.forward) == 0 || c.GetBlockID(pos + Vector3.forward) == 7)
            AddForwardFace(pos, c.GetBlockID(pos));
    }

    public void AddRightFace(Vector3 pos, short blockID)
    {
        float x = pos.x;
        float y = pos.y;
        float z = pos.z;

        verts.AddRange(new List<Vector3>{
            new Vector3 (x+0, y+0, z+0),
            new Vector3 (x+0, y+1, z+0),
            new Vector3 (x+0, y+1, z+1),
            new Vector3 (x+0, y+0, z+1)
        });

        vertCount += 4;

        trigs.AddRange(new List<int>{
            vertCount-4, vertCount-2, vertCount-3,
            vertCount-4, vertCount-1, vertCount-2,
        });

        Vector2 bl = GetAtlasBottomLeft(blockID);

        uvs.AddRange(new List<Vector2>{
            new Vector2(bl.x+ATLAS_BLOCK,bl.y),
            new Vector2(bl.x+ATLAS_BLOCK,bl.y+ATLAS_BLOCK),
            new Vector2(bl.x,bl.y+ATLAS_BLOCK),
            new Vector2(bl.x,bl.y),
        });
    }

    public void AddLeftFace(Vector3 pos, short blockID)
    {
        float x = pos.x;
        float y = pos.y;
        float z = pos.z;

        verts.AddRange(new List<Vector3>{
            new Vector3 (x+1, y+0, z+0),
            new Vector3 (x+1, y+1, z+0),
            new Vector3 (x+1, y+1, z+1),
            new Vector3 (x+1, y+0, z+1)
        });

        vertCount += 4;

        trigs.AddRange(new List<int>{
            vertCount-4, vertCount-3, vertCount-2,
            vertCount-4, vertCount-2, vertCount-1,
        });

        Vector2 bl = GetAtlasBottomLeft(blockID);

        uvs.AddRange(new List<Vector2>{
            new Vector2(bl.x,bl.y),
            new Vector2(bl.x,bl.y+ATLAS_BLOCK),
            new Vector2(bl.x+ATLAS_BLOCK,bl.y+ATLAS_BLOCK),
            new Vector2(bl.x+ATLAS_BLOCK,bl.y),
        });

    }

    public void AddBackFace(Vector3 pos, short blockID)
    {
        float x = pos.x;
        float y = pos.y;
        float z = pos.z;

        verts.AddRange(new List<Vector3>{
            new Vector3 (x+0, y+0, z+0),
            new Vector3 (x+1, y+0, z+0),
            new Vector3 (x+1, y+1, z+0),
            new Vector3 (x+0, y+1, z+0),
        });

        vertCount += 4;

        trigs.AddRange(new List<int>{
            vertCount-4, vertCount-2, vertCount-3,
            vertCount-4, vertCount-1, vertCount-2,
        });

        Vector2 bl = GetAtlasBottomLeft(blockID);

        uvs.AddRange(new List<Vector2>{
            new Vector2(bl.x,bl.y),
            new Vector2(bl.x+ATLAS_BLOCK,bl.y),
            new Vector2(bl.x+ATLAS_BLOCK,bl.y+ATLAS_BLOCK),
            new Vector2(bl.x,bl.y+ATLAS_BLOCK),
        });
    }

    public void AddForwardFace(Vector3 pos, short blockID)
    {
        float x = pos.x;
        float y = pos.y;
        float z = pos.z;

        verts.AddRange(new List<Vector3>{
            new Vector3 (x+0, y+0, z+1),
            new Vector3 (x+1, y+0, z+1),
            new Vector3 (x+1, y+1, z+1),
            new Vector3 (x+0, y+1, z+1),
        });

        vertCount += 4;

        trigs.AddRange(new List<int>{
            vertCount-4, vertCount-2, vertCount-1,
            vertCount-4, vertCount-3, vertCount-2,
        });

        Vector2 bl = GetAtlasBottomLeft(blockID);

        uvs.AddRange(new List<Vector2>{
            new Vector2(bl.x+ATLAS_BLOCK,bl.y),
            new Vector2(bl.x,bl.y),
            new Vector2(bl.x,bl.y+ATLAS_BLOCK),
            new Vector2(bl.x+ATLAS_BLOCK,bl.y+ATLAS_BLOCK),
        });

    }

    public void AddTopFace(Vector3 pos, short blockID)
    {
        float x = pos.x;
        float y = pos.y;
        float z = pos.z;

        verts.AddRange(new List<Vector3>{
            new Vector3 (x+1, y+1, z+0),
            new Vector3 (x+0, y+1, z+0),
            new Vector3 (x+0, y+1, z+1),
            new Vector3 (x+1, y+1, z+1),
        });

        vertCount += 4;

        trigs.AddRange(new List<int>{
            vertCount-4, vertCount-2, vertCount-1,
            vertCount-4, vertCount-3, vertCount-2,
        });

        Vector2 bl = GetAtlasBottomLeft(blockID);

        uvs.AddRange(new List<Vector2>{
            new Vector2(bl.x,bl.y+ATLAS_BLOCK),
            new Vector2(bl.x+ATLAS_BLOCK,bl.y+ATLAS_BLOCK),
            new Vector2(bl.x+ATLAS_BLOCK,bl.y),
            new Vector2(bl.x,bl.y),
        });
    }

    public void AddBottomFace(Vector3 pos, short blockID)
    {
        float x = pos.x;
        float y = pos.y;
        float z = pos.z;

        verts.AddRange(new List<Vector3>{
            new Vector3 (x+1, y+0, z+0),
            new Vector3 (x+0, y+0, z+0),
            new Vector3 (x+0, y+0, z+1),
            new Vector3 (x+1, y+0, z+1),
        });

        vertCount += 4;

        trigs.AddRange(new List<int>{
            vertCount-4, vertCount-1, vertCount-2,
            vertCount-4, vertCount-2, vertCount-3,
        });

        Vector2 bl = GetAtlasBottomLeft(blockID);

        uvs.AddRange(new List<Vector2>{
            new Vector2(bl.x,bl.y+ATLAS_BLOCK),
            new Vector2(bl.x+ATLAS_BLOCK,bl.y+ATLAS_BLOCK),
            new Vector2(bl.x+ATLAS_BLOCK,bl.y),
            new Vector2(bl.x,bl.y),
        });

    }

    public void generateGreedyMeshUp(Chunk c, short y)
    {
        // x and z traverse
        short[,] slice = new short[Chunk.WIDTH, Chunk.WIDTH];

        for (int x = 0; x < Chunk.WIDTH; x++)
        {
            for (int z = 0; z < Chunk.WIDTH; z++)
            {
                // if our block is air or above us is opaque... don't bother
                if (c.GetBlockID(new Vector3(x, y, z)) == 0 || Block.isOpaqueByID(c.GetBlockID(new Vector3(x, y + 1, z))))
                {
                    slice[x, z] = 0;
                }
                else
                {
                    slice[x, z] = c.GetBlockID(new Vector3(x, y, z));
                }
            }
        }

        for (int x = 0; x < Chunk.WIDTH; x++)
        {
            for (int z = 0; z < Chunk.WIDTH; z++)
            {
                short id = slice[x, z];
                // begin meshing
                if (id != 0)
                {
                    int xTrace = x;
                    int zTrace = z;

                    // Look forward one
                    while (xTrace + 1 < Chunk.WIDTH && slice[xTrace + 1, zTrace] == id)
                    {
                        xTrace++;
                        slice[xTrace, zTrace] = 0;
                    }

                    bool nextZIsValid = true;

                    while (zTrace +1 < Chunk.WIDTH)
                    {
                        for (int xCheck = x; xCheck <= xTrace; xCheck++)
                        {
                            if (slice[xCheck, zTrace+1] != id)
                            {
                                // we are not good
                                nextZIsValid = false;
                                break;
                            }
                        }

						if(!nextZIsValid) {
							break;
						}

						zTrace++;

                        // All our next z is valid
                        for (int xCheck = x; xCheck <= xTrace; xCheck++)
                        {
							// We mesh each now.
                            slice[xCheck,zTrace] = 0;
                        }

                    }

                    MeshUp(x, z, xTrace, zTrace, y, id);
                }
            }
        }
    }


    public void generateGreedyMeshDown(Chunk c, short y)
    {
        // x and z traverse
        short[,] slice = new short[Chunk.WIDTH, Chunk.WIDTH];

        for (int x = 0; x < Chunk.WIDTH; x++)
        {
            for (int z = 0; z < Chunk.WIDTH; z++)
            {
                // if our block is air or above us is opaque... don't bother
                if (c.GetBlockID(new Vector3(x, y, z)) == 0 || Block.isOpaqueByID(c.GetBlockID(new Vector3(x, y - 1, z))))
                {
                    slice[x, z] = 0;
                }
                else
                {
                    slice[x, z] = c.GetBlockID(new Vector3(x, y, z));
                }
            }
        }

        for (int x = 0; x < Chunk.WIDTH; x++)
        {
            for (int z = 0; z < Chunk.WIDTH; z++)
            {
                short id = slice[x, z];
                // begin meshing
                if (id != 0)
                {
                    int xTrace = x;
                    int zTrace = z;

                    // Look forward one
                    while (xTrace + 1 < Chunk.WIDTH && slice[xTrace + 1, zTrace] == id)
                    {
                        xTrace++;
                        slice[xTrace, zTrace] = 0;
                    }

                    bool nextZIsValid = true;

                    while (zTrace +1 < Chunk.WIDTH)
                    {
                        for (int xCheck = x; xCheck <= xTrace; xCheck++)
                        {
                            if (slice[xCheck, zTrace+1] != id)
                            {
                                // we are not good
                                nextZIsValid = false;
                                break;
                            }
                        }

						if(!nextZIsValid) {
							break;
						}

						zTrace++;

                        // All our next z is valid
                        for (int xCheck = x; xCheck <= xTrace; xCheck++)
                        {
							// We mesh each now.
                            slice[xCheck,zTrace] = 0;
                        }

                    }

                    MeshDown(x, z, xTrace, zTrace, y, id);
                }
            }
        }
    }


    void MeshUp(int startX, int startZ, int endX, int endZ, int y, short blockID)
    {
		//print(y+": mesh up "+startX+","+startZ+" "+endX+","+endZ);
        verts.AddRange(new List<Vector3>{
            new Vector3 (endX+1, y+1, startZ),
            new Vector3 (startX, y+1, startZ),
            new Vector3 (startX, y+1, endZ+1),
            new Vector3 (endX+1, y+1, endZ+1),
        });

        vertCount += 4;

        trigs.AddRange(new List<int>{
            vertCount-4, vertCount-2, vertCount-1,
            vertCount-4, vertCount-3, vertCount-2,
        });

        Vector2 bl = GetAtlasBottomLeft(blockID);

        uvs.AddRange(new List<Vector2>{
            new Vector2(bl.x,bl.y+ATLAS_BLOCK),
            new Vector2(bl.x+ATLAS_BLOCK,bl.y+ATLAS_BLOCK),
            new Vector2(bl.x+ATLAS_BLOCK,bl.y),
            new Vector2(bl.x,bl.y),
        });
    }


    void MeshDown(int startX, int startZ, int endX, int endZ, int y, short blockID)
    {
        verts.AddRange(new List<Vector3>{
            new Vector3 (endX+1, y, startZ),
            new Vector3 (startX, y, startZ),
            new Vector3 (startX, y, endZ+1),
            new Vector3 (endX+1, y, endZ+1),
        });

        vertCount += 4;

        trigs.AddRange(new List<int>{
            vertCount-4,  vertCount-1, vertCount-2,
            vertCount-4,  vertCount-2, vertCount-3,
        });

        Vector2 bl = GetAtlasBottomLeft(blockID);

        uvs.AddRange(new List<Vector2>{
            new Vector2(bl.x,bl.y+ATLAS_BLOCK),
            new Vector2(bl.x+ATLAS_BLOCK,bl.y+ATLAS_BLOCK),
            new Vector2(bl.x+ATLAS_BLOCK,bl.y),
            new Vector2(bl.x,bl.y),
        });
    }


    public static Vector2 GetAtlasBottomLeft(short blockID)
    {

        blockID -= 1;

        int x = blockID % ATLAS_WIDTH;
        int y = blockID / ATLAS_WIDTH;

        return new Vector2(((float)x * ATLAS_FULL_BLOCK) + ATLAS_PADDING, (1f - ATLAS_FULL_BLOCK) - ((float)y * ATLAS_FULL_BLOCK) + ATLAS_PADDING);
    }
}