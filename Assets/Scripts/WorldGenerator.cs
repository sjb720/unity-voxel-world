
// Generates the world and saves it into a world class.
using UnityEngine;

public class WorldGenerator
{

    int seed;
    
    public WorldGenerator(int seed) {
        this.seed = seed;
        Random.InitState(seed);
    }
    

    public short[,,] GenerateChunk(int chunkX, int chunkZ) {

        short[,,] chunk = new short[Chunk.WIDTH,Chunk.HEIGHT,Chunk.WIDTH];

        float [,] hillsFinal = new float[Chunk.WIDTH,Chunk.WIDTH];

        for(int passes = 1; passes < 4; passes++){
            float hillScale = 0.01f*passes;
            float hillAmp = 0.05f/passes;

            float[,] hills = SimplexNoise.Noise.Calc2D(chunkX*Chunk.WIDTH, chunkZ*Chunk.WIDTH, Chunk.WIDTH, Chunk.WIDTH, hillScale);

            for(int x = 0; x < Chunk.WIDTH; x++) {
                for(int z = 0; z < Chunk.WIDTH; z++) {
                    if(passes == 0) {
                        hillsFinal[x,z] =  (hillAmp*hills[x,z]);
                    } else {
                        hillsFinal[x,z] += (hillAmp*hills[x,z]);
                    }
                }
            } 
            
        }

        for(int x = 0; x < Chunk.WIDTH; x++) {
                for(int z = 0; z < Chunk.WIDTH; z++) {
                    int y = (int)hillsFinal[x,z]+100;
                    int fromTop = 0;
                    int grassDepth = Random.Range(2,4);

                    while(y > 0) {
                        if(fromTop == 0) {
                            if(y+1 < Chunk.HEIGHT) {
                                int odds = Random.Range(0,4);
                                if(odds == 0) {
                                    // adds flowers
                                    //chunk[x,y+1,z] = 7;
                                }
                            }

                            chunk[x,y,z] = 4;
                        }
                        else if(fromTop <= grassDepth) {
                            chunk[x,y,z] = 2;
                        } else {
                            int odds = Random.Range(0,5);
                            if(odds == 0) {
                                chunk[x,y,z] = 5;
                            } else {
                                chunk[x,y,z] = 1;
                            }
                        }
                        y--;
                        fromTop++;
                    }
                }
            } 

        float scale = 0.02f;
        SimplexNoise.Noise.Seed = seed;
        float[,,] simplex = SimplexNoise.Noise.Calc3D(chunkX*Chunk.WIDTH, chunkZ*Chunk.WIDTH, Chunk.WIDTH,Chunk.HEIGHT,Chunk.WIDTH, scale);

        byte threshold = 180;

        for(int x = 0; x < Chunk.WIDTH; x++) {
            for(int y = 0; y < Chunk.HEIGHT; y++) {
                for(int z = 0; z < Chunk.WIDTH; z++) {
                    if(y < 3) {
                        chunk[x,y,z] = 6;
                    }
                    else if(simplex[x,y,z] > threshold) {
                        chunk[x,y,z] = 0;
                    }
                }
            }
        }


        return chunk;

    }
    
}
