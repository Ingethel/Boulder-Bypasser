using UnityEngine;
using System;
using System.Collections;

public class CollectibleManager : MonoBehaviour {

    public Collectible[] collecctibles;
    public float spawnInterval;

    PoolManager pool;
    BoulderSpawnManager boulderManager;
    GameManager manager;

	void Start () {
        pool = PoolManager.instance;
        boulderManager = FindObjectOfType<BoulderSpawnManager>();

        manager = FindObjectOfType<GameManager>();
        manager.InGameEvent += StartRoutine;
        manager.EndGameEvent += StopRoutine;

        foreach (Collectible c in collecctibles)
            pool.CreatePool(c.obj, c.number);
    }

    public void StartRoutine() {
        StartCoroutine(SpawnCollectible(spawnInterval));
    }

    public void StopRoutine() {
        StopAllCoroutines();
    }

    IEnumerator SpawnCollectible(float seconds)
    {
        while (true) {
            pool.SpawnObject(collecctibles[0].obj, boulderManager.GetSafePoint());
            yield return new WaitForSeconds(seconds);
        }
    }
}

[Serializable]
public class Collectible {
    public GameObject obj;
    public int number;
}