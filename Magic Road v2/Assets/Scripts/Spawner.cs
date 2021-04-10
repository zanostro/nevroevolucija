using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class Spawner : MonoBehaviour
{
    GameObject[,] spawnedTile;
    List<Tile> possibleTiles;

    WaveElement[,] wave;

    // Start is called before the first frame update
    void Start()
    {
        WFC wfc = (WFC)GetComponent("WFC");
        spawnedTile = new GameObject[wfc.dimensions.x, wfc.dimensions.y];
        possibleTiles = wfc.PossibleTiles;
        wave = wfc.Wave;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    void SpawnWave()
    {
        for (int x = 0; x < wave.GetLength(0); x++)
        {
            for (int y = 0; y < wave.GetLength(1); y++)
            {
                SpawnTile(x, y);
            }
        }
    }
    

    void SpawnTile(int x, int y) {
        
        GameObject.Destroy(spawnedTile[x, y]);

        if (wave[x, y].coeffSum == 1)
        {
            Tile tile = possibleTiles[wave[x, y].getTileIdx()];
            spawnedTile[x, y] = GameObject.Instantiate(tile.gameObject, new Vector3(x, 0, y), Quaternion.Euler(0, (float)tile.rotation * 90, 0));
        }
        else if (wave[x, y].coeffSum == 0)
        {
            spawnedTile[x, y] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            float scale = .5f;
            spawnedTile[x, y].transform.position = new Vector3(x, 0, y);
            spawnedTile[x, y].transform.localScale = new Vector3(scale, scale, scale);
        }
        else
        {
            spawnedTile[x, y] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            float scale = (float)wave[x, y].entropy / wave[x, y].coeff.Length;
            spawnedTile[x, y].transform.position = new Vector3(x, 0, y);
            spawnedTile[x, y].transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
*/
