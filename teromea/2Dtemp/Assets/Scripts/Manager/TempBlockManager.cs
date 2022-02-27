using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBlockManager : SingletonMonoBehaviour<TempBlockManager>
{

    public List<GameObject> tempblocks = new List<GameObject>();
    public GameObject prefabobj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CleateTempBlock (Vector3 pos) 
    {
        if(!PhysicsFunc.isThereAnyObjectOnThePoint(1 << 6 | 1 << 7))
        {
            tempblocks.Add(Instantiate(prefabobj, pos, Quaternion.identity));
            return true;
        } 
        else 
        {
            return false;
        }
    }

    public bool DeleteTempBlock(GameObject obj)
    {
        Destroy(obj);
        return tempblocks.Remove(obj);
    }
}
