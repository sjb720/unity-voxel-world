using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public GameObject chunkRenderer;

    public static int WORLD_RADIUS = 15;

    public static Chunk[,] world = new Chunk[WORLD_RADIUS,WORLD_RADIUS];

    WorldGenerator worldGen;

    GameObject[,] chunkRenderers = new GameObject[WORLD_RADIUS,WORLD_RADIUS];

    void Awake() {
    }
    
    // Start is called before the first frame update
    void Start()
    {
        worldGen = new WorldGenerator(123456);

        for(int x = 0; x < WORLD_RADIUS; x++) {
            for(int z= 0; z < WORLD_RADIUS; z++) {
                CreateChunk(x,z);
            }
        }

        for(int x = 0; x < WORLD_RADIUS; x++) {
            for(int z= 0; z < WORLD_RADIUS; z++) {
                RenderChunk(x,z);
            }
        }

    }

    void CreateChunk(int x, int z) {
        Chunk chunk = new Chunk(worldGen.GenerateChunk(x,z),x,z);
        world[x,z] = chunk;

    }
    
    void RenderChunk(int x, int z) {
        Chunk chunk = world[x,z];

        GameObject cr = Instantiate(chunkRenderer, new Vector3(x*Chunk.WIDTH,0,z*Chunk.WIDTH),Quaternion.identity);

        WorldRenderer wr = cr.GetComponent<WorldRenderer>();
        chunk.ForEachBlock(CreateBlock, wr, chunk);    

        for(short y = 0; y < Chunk.HEIGHT; y++) {
            wr.generateGreedyMeshUp(chunk, y);
            wr.generateGreedyMeshDown(chunk,y);
        }

        wr.RenderWorld();
    }

    void CreateBlock(Block block, Chunk c, WorldRenderer wr) {
        if(block.id != 0 && block.id != 7)
            wr.AddCube(new Vector3(block.localX,block.localY,block.localZ), c);
        
        if(block.id == 7) {
            wr.AddPlant(new Vector3(block.localX,block.localY,block.localZ), c);
        }

    }


    void PrintBlock(Block block) {
         print("x: "+block.localZ+", z: "+block.localZ);
    }
}
