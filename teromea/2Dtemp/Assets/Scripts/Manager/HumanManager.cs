using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanManager : SingletonMonoBehaviour<HumanManager>
{

    public List<GameObject> humanList = new List<GameObject>();
    public GameObject prefabobj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CleateHuman (Vector3 pos) 
    {
        if (humanList.Count <= Model.MAX_TEMPBLOCKS)
            humanList.Add(Instantiate(prefabobj, pos, Quaternion.identity));
    }

    public void DeleteHuman(GameObject obj)
    {
        Destroy(obj);
        humanList.Remove(obj);
    }
}
