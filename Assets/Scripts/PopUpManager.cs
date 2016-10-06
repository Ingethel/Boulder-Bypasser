using UnityEngine;
using System;

public class PopUpManager : MonoBehaviour {

    public PopUpText[] popUps;
    PoolManager pool;
    public GameObject InGameUI;

    void Start () {
        pool = PoolManager.instance;
        foreach (PopUpText c in popUps)
            pool.CreatePool_CustomParent(c.obj, c.number, InGameUI.transform);
    }
	
    public void SpawnScorePopUp(int index, int score, Vector3 pos)
    {
        GameObject o = popUps[index].obj;
        PopUpTextScript oTScript = o.GetComponent<PopUpTextScript>();
        pool.SpawnObject(o, pos);
        if (oTScript)
            oTScript.SetText(score.ToString());
    }
}

[Serializable]
public class PopUpText
{
    public GameObject obj;
    public int number;
}