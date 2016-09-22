using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BoulderSpawnManager : MonoBehaviour
{

    [Header("Grid Size")]
    public int xPoints;
    public int yPoints;
    public float xPos, yPos, zPos, stepSize;

    [Header("Spawnable Objets")]
    public MultiObject[] multiPools;

    public int MaxSpawnsInQueue;
    public int CurrentSpawns { get; private set; }

    [Header("Spawn Variables")]
    public float spawnDelayMax;
    public float spawnDelayMin, safeAreaRadius;
    [Range(0, 1)]
    public float lowProb, highProb;
    float spawnDelay;

    Vector2 safePoint;
    List<Cluster> clusters;
    Queue<NewSpawn> spawns;
    Cell[,] arrayMap;
    
    PoolManager pool;
    CaveManager caveManager;
    GameManager manager;

    void Awake()
    {
        pool = PoolManager.instance;
        for (int i = 0; i < multiPools.Length; i++)
        {
            pool.CreateMultiPool(multiPools[i].obj, multiPools[i].probabilities, multiPools[i].totalSize, i, "PoolObject_" + i.ToString());
        }
        safePoint = new Vector2(xPoints / 2, yPoints / 2);
        spawns = new Queue<NewSpawn>();
        initialiseArray();
        clusters = new List<Cluster>();
        spawnDelay = spawnDelayMax;
    }

    void Start()
    {
        caveManager = FindObjectOfType<CaveManager>();
        manager = FindObjectOfType<GameManager>();
        manager.InGameEvent += StartRoutine;
        manager.EndGameEvent += StopRoutine;
    }

    void FixedUpdate()
    {
        Vector2 offset = caveManager.GetWaveOffset();
        Vector3 pos = Vector3.zero;
        pos.x += offset.x;
        pos.y += offset.y;
        transform.position = pos;
    }

    public void StartRoutine()
    {
        StartCoroutine(SpawnBoulder());
        StartCoroutine(SpawnDelayManager());
    }

    public void StopRoutine()
    {
        StopAllCoroutines();
    }

    IEnumerator SpawnDelayManager()
    {
        while (spawnDelay > spawnDelayMin)
        {
            spawnDelay -= Time.deltaTime;
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    IEnumerator SpawnBoulder()
    {
        while (true)
        {
            if (spawns.Count > 0)
            {
                NewSpawn s = spawns.Dequeue();
                s.position += transform.position;
                if (s.magnitude < 30)
                    pool.SpawnObject(0, s.position, UnityEngine.Random.rotation, Vector3.one * (s.magnitude + 7));
                else
                    pool.SpawnObject(1, s.position, Quaternion.identity, Vector3.one * 12);
            }

            if (spawns.Count < MaxSpawnsInQueue / 2)
            {
                updateBoulderSpawns();
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public void updateBoulderSpawns()
    {
        updateSafePoint();
        updateSpawnPoints();
    }

    void initialiseArray()
    {
        arrayMap = new Cell[xPoints, yPoints];
        for (int y = 0; y < yPoints; y++)
        {
            for (int x = 0; x < xPoints; x++)
            {
                arrayMap[x, y] = new Cell();
                arrayMap[x, y].prob = highProb;
                arrayMap[x, y].spawns = false;
                arrayMap[x, y].canSpawn = true;
                arrayMap[x, y].clusterID = -1;
                arrayMap[x, y].worldPosition = new Vector3(xPos + stepSize * x, yPos + stepSize * y, zPos);
            }
        }
        // deactivate corners
        arrayMap[0, 0].canSpawn = false;
        arrayMap[0, 1].canSpawn = false;
        arrayMap[0, yPoints - 1].canSpawn = false;
        arrayMap[0, yPoints - 2].canSpawn = false;
        arrayMap[1, 0].canSpawn = false;
        arrayMap[1, yPoints - 1].canSpawn = false;
        arrayMap[xPoints - 2, 0].canSpawn = false;
        arrayMap[xPoints - 2, yPoints - 1].canSpawn = false;
        arrayMap[xPoints - 1, 0].canSpawn = false;
        arrayMap[xPoints - 1, 1].canSpawn = false;
        arrayMap[xPoints - 1, yPoints - 1].canSpawn = false;
        arrayMap[xPoints - 1, yPoints - 2].canSpawn = false;

    }

    void updateSpawnPoints()
    {

        for (int y = 0; y < yPoints; y++)
        {
            for (int x = 0; x < xPoints; x++)
            {
                // update spawn probabilities
                if (Vector2.Distance(new Vector2(x, y), safePoint) < safeAreaRadius)
                    arrayMap[x, y].prob = lowProb;
                else
                    arrayMap[x, y].prob = highProb;

                // reset values
                arrayMap[x, y].spawns = false;
                arrayMap[x, y].clusterID = -1;

                // check for new spawn
                if (arrayMap[x, y].canSpawn)
                {
                    arrayMap[x, y].spawns = UnityEngine.Random.value < arrayMap[x, y].prob;
                    if (arrayMap[x, y].spawns)
                    {
                        // find near cluster ids
                        int c1 = -1, c2 = -1;
                        if (x - 1 > 0)
                            c1 = arrayMap[x - 1, y].clusterID;
                        if (y - 1 > 0)
                            c2 = arrayMap[x, y - 1].clusterID;

                        int id = 0;
                        // if no cluster nearby create new one
                        if (c1 == -1 && c2 == -1)
                        {
                            id = clusters.Count;
                            clusters.Add(new Cluster(id));
                        }
                        else if (c1 != -1 && c2 == -1)
                        {
                            id = c1;
                        }
                        else if (c1 == -1 && c2 != -1)
                        {
                            id = c2;
                        }
                        else if (c1 != -1 && c2 != -1)
                        {
                            if (c1 != c2)
                            {
                                int id_old = clusters.Count;
                                clusters.Add(new Cluster(id));

                                for (int i = 0; i < clusters[c1].cells.Count; i++)
                                {
                                    clusters[id_old].cells.Add(clusters[c1].cells[i]);
                                }
                                for (int i = 0; i < clusters[c2].cells.Count; i++)
                                {
                                    clusters[id_old].cells.Add(clusters[c2].cells[i]);
                                }

                                if (c1 > c2)
                                {
                                    clusters.Remove(clusters[c1]);
                                    clusters.Remove(clusters[c2]);

                                }
                                else
                                {
                                    clusters.Remove(clusters[c2]);
                                    clusters.Remove(clusters[c1]);

                                }

                                for (id = c1 < c2 ? c1 : c2; id < clusters.Count; id++)
                                {
                                    clusters[id].index = id;
                                    for (int j = 0; j < clusters[id].cells.Count; j++)
                                    {
                                        clusters[id].cells[j].clusterID = id;
                                    }
                                }
                                id = clusters.Count - 1;
                            }
                            else
                            {
                                id = c1;
                            }

                        }
                        arrayMap[x, y].clusterID = id;
                        clusters[id].cells.Add(arrayMap[x, y]);
                    }
                }

            }
        }

        while (clusters.Count > 0)
        {
            Cluster c = RemoveItem(UnityEngine.Random.Range(0, clusters.Count));
            if (c != null)
            {
                spawns.Enqueue(new NewSpawn(c.cells.Count, c.worldPosition));
            }
        }

        if (highProb < 1 - lowProb)
            highProb += 0.01f;
    }

    void updateSafePoint()
    {
        int x = UnityEngine.Random.Range(-3, 3);
        int y = UnityEngine.Random.Range(-3, 3);

        int tempX = (int)safePoint.x + x;
        int tempY = (int)safePoint.y + y;

        safePoint.x = (tempX >= 0 && tempX < xPoints) ? tempX : safePoint.x - x;
        safePoint.y = (tempY >= 0 && tempY < yPoints) ? tempY : safePoint.y - y;
    }


    Cluster RemoveItem(int i)
    {
        Cluster returnCluster;
        if (i < clusters.Count)
        {
            returnCluster = clusters[i];
            clusters.RemoveAt(i);
            return returnCluster;
        }
        else
        {
            return null;
        }
    }

    public Vector3 GetSafePoint()
    {
        return new Vector3(safePoint.x, safePoint.y, zPos) + transform.position;
    }
}

public class Cell
{
    public float prob;
    public bool spawns;
    public bool canSpawn;
    public int clusterID;
    public Vector3 worldPosition;
}

public class Cluster
{
    public int index;
    public List<Cell> cells;
    public Vector3 worldPosition
    {
        get
        {
            Vector3 avrg = Vector3.zero;
            foreach (Cell c in cells)
            {
                avrg += c.worldPosition;
            }
            avrg /= cells.Count;
            return avrg;
        }
    }

    public Cluster(int i)
    {
        cells = new List<Cell>();
        index = i;
    }
}

public struct NewSpawn
{
    public int magnitude;
    public Vector3 position;

    public NewSpawn(int mag, Vector3 pos)
    {
        magnitude = mag;
        position = pos;
    }
}

[Serializable]
public class MultiObject
{
    public GameObject[] obj;
    [Range(0, 1)]
    public float[] probabilities;
    public int totalSize;
}