using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

public class WFC : MonoBehaviour
{
    public int parentCounter = 2;
    public int nornalCounter = 0;
    public int constructorCounter = 0;

    private int semiNormals = 5;
    private int constructors = 5;
  


    public static Vector3 spawnPosition = new Vector3(0, 2, 0);

    int iteration = 0;

    public int sampleSize; //min 20
    public List<GameObject> players = new List<GameObject>();
    bool carsSpawned = false;

    public GameObject playerPrefab;
    public GameObject finishline;


    public Vector2Int dimensions;

    Vector2Int startPos;
    Vector2Int endPos;

    public bool useSeed;
    public int seed=0;

    public Tile startTile;
    public Tile rotatedStartTile;

    Transform megaTile;

    Transform[,] infinityTiling = new Transform[3, 3];

    public List<WeightedTile> inputTiles = new List<WeightedTile>();

    List<WeightedTile> possibleTiles = new List<WeightedTile>();

    WaveElement[,] wave;

    Tile[,] spawnedTile;

    //float sumWeights = 0;

    Queue<Vector2Int> spawnOrder = new Queue<Vector2Int>();



    //moje -------------------------------------
    void spawnPlayers()
    {
        for (int i = 0; i < sampleSize; i++)
        {
            var temp = Instantiate(playerPrefab, spawnPosition, Quaternion.identity).transform;
            GameObject player = temp.gameObject;

            player.GetComponent<ScoreCounter>().StartCounting();
            players.Add(player);
          
 }

        carsSpawned = true;
    }
    //-----------------------------------------------


    void Start()
    {
        Physics.IgnoreLayerCollision(9, 9);     //ignorira kolizijo med posameznimi avtomobili

        if (useSeed) Random.InitState(seed);
        if (sampleSize < 20) sampleSize = 20;
        bool validPath = false;
        while (!validPath)
        {
            bool successfulCollapse = false;
            while (!successfulCollapse)
            {

                megaTile = new GameObject().transform;
                megaTile.SetParent(transform);
                megaTile.name = "megaTile";

                InitPossibleTiles();

                wave = new WaveElement[dimensions.x, dimensions.y];
                spawnedTile = new Tile[dimensions.x, dimensions.y];

                spawnOrder.Clear();

                InitWave();

                SpawnStartAndFinish();

                //StartCoroutine(CollapseAll());
                successfulCollapse = CollapseAll();
                if (!successfulCollapse)
                {
                    seed++;
                    if (useSeed) Random.InitState(seed);
                    //Debug.Log("collapse unsuccessful");
                }
            }
            validPath = Pathfinder.PathExists(spawnedTile, startPos, endPos);
            if (!validPath)
            {
                seed++;
                if (useSeed) Random.InitState(seed);
                //Debug.Log("pathfind");
            }
        }
       // Debug.Log(seed);

        StartCoroutine(SpawnTilesAndStartGame());

    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        if (carsSpawned)
        {
            int playerCount = players.Count;
            if (playerCount == 0) spawnPlayers();

            bool engineState = false;

            for(int i = 0; i< playerCount && !engineState; i++)
            {
                engineState = players[i].GetComponent<Global>().isActive;
            }
            if (!engineState)          //se vsi avti stojijo
            {
                constructorCounter = 0;
                nornalCounter = 0;


                //best player
                int bestIndex = findBestPlayer();
                GameObject bestPlayer = players[bestIndex];
                players.RemoveAt(bestIndex);


                //second best
                bestIndex = findBestPlayer();
                GameObject secondBestPlayer = players[bestIndex];
                players.RemoveAt(bestIndex);


                carsSpawned = false;

                iteration++;
                Debug.Log("Itteration n: " + iteration +
                    " best score: " + bestPlayer.GetComponent<ScoreCounter>().score
                    + " second: "
                    + secondBestPlayer.GetComponent<ScoreCounter>().score
                    );


                makeNewPlayers(bestPlayer, secondBestPlayer);
                destroyPlayers();

                //lahko to zbrises in vids history best playerjev
                Destroy(bestPlayer.gameObject);
                Destroy(secondBestPlayer.gameObject);



                carsSpawned = true;
            }
        }
        
    }
    int findBestPlayer()
    {
        int counter = 0;
        GameObject bestPlayer = players[0];              
        for (int i = 0; i<players.Count; i++)
        {
            GameObject temp = players[i];
            if (temp.GetComponent<ScoreCounter>().score > bestPlayer.GetComponent<ScoreCounter>().score)
            {
                bestPlayer = temp;
                counter = i;
            }
               
        }
        return counter;
    }




    void destroyPlayers()
    {
        for(int i = 0; i<sampleSize-2; i++)
        {
            GameObject tempPlayer = players[0];
            players.RemoveAt(0);
            Destroy(tempPlayer.gameObject);
        }
    }

    
    void makeNewPlayers(GameObject bestPlayer, GameObject secondBestPlayer)
    {
        createBestPlayer(bestPlayer);
        createBestPlayer(secondBestPlayer);
       

        for (int i = 0; i<sampleSize-2; i++)
        {
            Transform temp = GameObject.Instantiate(playerPrefab, spawnPosition, Quaternion.identity).transform;
            GameObject player = temp.gameObject;

            player.transform.position = spawnPosition;
            player.transform.rotation = new Quaternion(0, 0, 0, 0);

            player.GetComponent<Global>().cloneNetwork(bestPlayer.GetComponent<Global>().network);

            switch (true)
            {
                //naredi iz 0
                case var expression when (i < constructors):
                    player.GetComponent<Global>().network = new Network(Global.networkStructure);
                    player.name = "Player_Constructor_";
                    break;
                
                //srednja mutacija
                case var expression when (i >= constructors && i < constructors + semiNormals):
                    nornalCounter++;
                    player.GetComponent<Global>().network.crossBreed(secondBestPlayer.GetComponent<Global>().network);
                    player.GetComponent<Global>().network.mutateNetwork(0.3);
                    player.name = "Player_MidMutate";
                    break;
                
                //child and mutation
                default:                
                    nornalCounter++;
                    player.GetComponent<Global>().network.crossBreed(secondBestPlayer.GetComponent<Global>().network);
                    player.GetComponent<Global>().network.mutateNetwork();
                    player.name = "Player_Mutate";
                    break;
            }
            

            player.GetComponent<ScoreCounter>().StartCounting();
            player.gameObject.SetActive(true);
            

            players.Add(player);
            player = null;

        }
        
    }

    void createBestPlayer(GameObject bestPlayer)
    {
        Transform temp = GameObject.Instantiate(playerPrefab, spawnPosition, Quaternion.identity).transform;
        GameObject player = temp.gameObject;
        player.transform.position = spawnPosition;
        player.transform.rotation = new Quaternion(0, 0, 0, 0);
        player.GetComponent<Global>().cloneNetwork(bestPlayer.GetComponent<Global>().network);
        player.GetComponent<ScoreCounter>().StartCounting();
        player.gameObject.SetActive(true);
        player.name = "Player_parent";

        players.Add(player.gameObject);
        player = null;

    }
    




    IEnumerator SpawnTilesAndStartGame()
    {
        while (spawnOrder.Count > 0)
        {
            Vector2Int spawnIdx = spawnOrder.Dequeue();
            UpdateWorldTile(spawnIdx.x, spawnIdx.y);
            spawnedTile[spawnIdx.x, spawnIdx.y].gameObject.SetActive(true);
            yield return null;
        }

        /*Transform finishCube = GameObject.Instantiate(finishline).transform;
        finishCube.position = new Vector3((endPos.x - dimensions.x / 2)*10, 10, (endPos.y - dimensions.y / 2)*10); */                                   
        //Debug.Log("finish position: " + endPos);
        
        yield return new WaitForSeconds(3f);
        
        

        BoxCollider finishCollider = spawnedTile[endPos.x, endPos.y].gameObject.AddComponent<BoxCollider>();
        finishCollider.size = new Vector3(10, 10, 10);
        finishCollider.center = new Vector3(0, 5, 0);
        finishCollider.isTrigger = true;
        finishCollider.tag = "Finish";


        
        



        yield return new WaitForSeconds(0.5f);

        spawnPlayers();
    }

    void SpawnStartAndFinish()
    {
        startPos = dimensions / 2;

        rotatedStartTile = GameObject.Instantiate(startTile);
        
        int rotation = Random.Range(0, 4);
        for (int i = 0; i < rotation; i++)
        {
            rotatedStartTile.Rotate();
        }
        rotatedStartTile.transform.SetParent(transform);
        rotatedStartTile.gameObject.SetActive(false);
        rotatedStartTile.name = startTile.name + " " + rotatedStartTile.rotation;


        WeightedTile weightedStartTile = new WeightedTile();
        weightedStartTile.tile = rotatedStartTile;
        weightedStartTile.weight = 1;
        List<WeightedTile> startTileList = new List<WeightedTile>();
        startTileList.Add(weightedStartTile);
        wave[startPos.x, startPos.y] = new WaveElement(startTileList);
        Collapse(startPos);


        endPos = new Vector2Int(Random.Range(0, dimensions.x), Random.Range(0, dimensions.y));
        while (Vector2Int.Distance(startPos, endPos) < Mathf.Max(dimensions.x, dimensions.y) / 4)
        {
            endPos = new Vector2Int(Random.Range(0, dimensions.x), Random.Range(0, dimensions.y));
        }
        wave[endPos.x, endPos.y].RemoveTilesNotOfType(Tile.TileType.road);
    }

    void InitPossibleTiles()
    {
        possibleTiles.Clear();
        //add tiles according to symmetry
        foreach (WeightedTile wTile in inputTiles)
        {
            Tile tile = wTile.tile;
            float weight = wTile.weight;

            Tile clone = GameObject.Instantiate(tile);
            clone.name = tile.name;

            switch (tile.symmetry)
            {
                case Tile.Symmetry.X:
                    AddPossibleTile(clone, weight);
                break;

                case Tile.Symmetry.I:
                    weight *= 0.5f;
                    for (int i = 0; i < 2; i++)
                    {
                        AddPossibleTile(clone, weight);
                        clone.Rotate();
                    }
                    break;

                case Tile.Symmetry.L:
                    weight *= 0.25f;
                    for(int i = 0; i < 4; i++)
                    {
                        AddPossibleTile(clone, weight);
                        clone.Rotate();
                    }
                break;
            }
            //clone.gameObject.SetActive(false);
            GameObject.Destroy(clone.gameObject);
        }
    }

    void AddPossibleTile(Tile original, float weight)
    {
        Tile newTile = GameObject.Instantiate(original);
        newTile.transform.SetParent(transform);
        newTile.gameObject.SetActive(false);
        newTile.name = original.name + " " + original.rotation;

        WeightedTile wTile;
        wTile.tile = newTile;
        wTile.weight = weight;

        possibleTiles.Add(wTile);

        //sumWeights += weight;
    }

    void InitWave()
    {
        for (int x = 0; x < wave.GetLength(0); x++)
        {
            for (int y = 0; y < wave.GetLength(1); y++)
            {
                wave[x, y] = new WaveElement(possibleTiles);
            }
        }
    }

    bool CollapseAll() //returns true if successful
    {
        Vector2Int collapsingIdx = findMinEntropy();
        while (collapsingIdx != -Vector2Int.one)
        {
            if (wave[collapsingIdx.x, collapsingIdx.y].coeffSum == 0) return false;

            int chosenIdx = Collapse(collapsingIdx);

            //float weightReduction = dimensions.x * dimensions.y //neki TODO
            //possibleTiles[chosenIdx]
            
            //UpdateWorldTile(collapsingIdx.x, collapsingIdx.y);
            //yield return null;

            collapsingIdx = findMinEntropy();
        }

        return true;
    }

    int Collapse(Vector2Int collapsingIdx)
    {
        spawnOrder.Enqueue(collapsingIdx);
        int chosenIdx = wave[collapsingIdx.x, collapsingIdx.y].Collapse();
        Propagate(collapsingIdx);
        UpdateWorldTile(collapsingIdx.x, collapsingIdx.y);

        return chosenIdx;
    }

    Vector2Int findMinEntropy()
    {
        Vector2Int idx = new Vector2Int(-1, -1);
        float minEntropy = float.MaxValue;
        for (int x = 0; x < wave.GetLength(0); x++)
        {
            for (int y = 0; y < wave.GetLength(1); y++)
            {
                if (!wave[x, y].collapsed && wave[x, y].entropy < minEntropy)  //??change
                {
                    idx = new Vector2Int(x, y);
                    minEntropy = wave[x, y].entropy;
                }
            }
        }
        return idx;
    }

    void Propagate(Vector2Int startPos)
    {
        Propagate(startPos.x, startPos.y);
    }

    void Propagate(int x, int y)
    {

        int xn = Wrap(x - 1, dimensions.x);
        int ye = Wrap(y + 1, dimensions.y);
        int xs = Wrap(x + 1, dimensions.x);
        int yw = Wrap(y - 1, dimensions.y);

        if (wave[xn, y].ChangeToMatch(wave[x, y], Tile.Direction.N))
        {
            if (wave[xn, y].coeffSum == 1) Collapse(new Vector2Int(xn, y));
            //UpdateWorldTile(xn, y);
        }
        if (wave[x, ye].ChangeToMatch(wave[x, y], Tile.Direction.E))
        {
            if (wave[x, ye].coeffSum == 1) Collapse(new Vector2Int(x, ye));
            //UpdateWorldTile(x, ye);
        }
        if (wave[xs, y].ChangeToMatch(wave[x, y], Tile.Direction.S))
        {
            if (wave[xs, y].coeffSum == 1) Collapse(new Vector2Int(xs, y));
            //UpdateWorldTile(xs, y);
        }
        if (wave[x, yw].ChangeToMatch(wave[x, y], Tile.Direction.W))
        {
            if (wave[x, yw].coeffSum == 1) Collapse(new Vector2Int(x, yw));
            //UpdateWorldTile(x, yw);
        }
    }

    int Wrap(int x, int max)
    {
        return (x + max) % max;
    }

    void UpdateWorldTile(int x, int y)
    {

        GameObject.Destroy(spawnedTile[x, y]);

        if(x == startPos.x && y == startPos.y)
        {
            GameObject tileGameObject = GameObject.Instantiate(rotatedStartTile.gameObject, new Vector3((x - dimensions.x / 2) * 10, 0, (y - dimensions.y / 2) * 10), Quaternion.Euler(0, (float)rotatedStartTile.rotation * 90, 0));
            spawnedTile[x, y] = tileGameObject.GetComponent<Tile>();
            spawnedTile[x, y].name = x + " " + y + " " + rotatedStartTile.name;
        }

        else if (wave[x, y].coeffSum == 1)
        {
            Tile tile = possibleTiles[wave[x, y].getTileIdx()].tile;
            GameObject tileGameObject = GameObject.Instantiate(tile.gameObject, new Vector3((x - dimensions.x / 2) * 10, 0, (y - dimensions.y / 2) * 10), Quaternion.Euler(0, (float)tile.rotation * 90, 0));
            spawnedTile[x, y] = tileGameObject.GetComponent<Tile>();
            spawnedTile[x, y].name = x + " " + y + " " + tile.name;
        }
        else if (wave[x, y].coeffSum == 0)
        {
            /*
            spawnedTile[x, y] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            float scale = .5f;
            spawnedTile[x, y].transform.position = new Vector3(x, 0, y);
            spawnedTile[x, y].transform.localScale = new Vector3(scale, scale, scale);
            

            spawnedTile[x, y].name = x + " " + y + " " + "failed to generate";
            */
        }
        else
        {
            /*
            spawnedTile[x, y] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            float scale = (float)wave[x, y].entropy / wave[x, y].coeff.Length;
            spawnedTile[x, y].transform.position = new Vector3(x, 0, y);
            spawnedTile[x, y].transform.localScale = new Vector3(scale, scale, scale);
            

            spawnedTile[x, y].name = x + " " + y + " " + "ungenerated " + wave[x, y].coeffSum;
            */
        }

        spawnedTile[x, y].transform.SetParent(megaTile);
        //spawnedTile[x, y].gameObject.SetActive(true);
    }
}
