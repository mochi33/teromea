using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBlockManager : SingletonMonoBehaviour<TempBlockManager>
{

    public GameObject[] tempblocks = new GameObject[Model.MAX_TEMPBLOCKS];
    public int tempblocklength;
    public GameObject prefabobj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CleateTempBlock (Vector3 pos) {
        if (tempblocklength < Model.MAX_TEMPBLOCKS)
            tempblocks[tempblocklength - 1] = Instantiate(prefabobj, pos, Quaternion.identity);
    }
}
