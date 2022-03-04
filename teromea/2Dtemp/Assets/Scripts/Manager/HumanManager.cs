using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanManager : SingletonMonoBehaviour<HumanManager>
{
    public List<GameObject> humanObjList = new List<GameObject>();
    public List<Human> humanList = new List<Human>();
    public GameObject prefabobj;
    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject humanObj in humanObjList)
        {
            humanList.Add(humanObj.GetComponent<Human>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CleateHuman (Vector3 pos) 
    {
        if (humanList.Count <= Model.MAX_TEMPBLOCKS)
        {
            GameObject humanObj = Instantiate(prefabobj, pos, Quaternion.identity);
            humanList.Add(humanObj.GetComponent<Human>());
        }
    }

    public void DeleteHuman(GameObject obj)
    {
        humanList.Remove(obj.GetComponent<Human>());
        Destroy(obj);
    }
}
