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

    ///<Summary>
    ///TempBlockを作成するときは必ずこのメソッドを使って欲しい。
    ///<Summary>
    public bool CleateTempBlock (Vector3 pos) 
    {
        if(!PhysicsFunc.isThereAnyObjectOnThePoint(1 << 6 | 1 << 7))
        {
            GameObject tempBlock = Instantiate(prefabobj, pos, Quaternion.identity);
            tempblocks.Add(tempBlock);
            SetSetBlockInstraction(tempBlock);
            return true;
        } 
        else 
        {
            return false;
        }

        void SetSetBlockInstraction(GameObject tempBlock)
        {
            InstractionManager.Instance.CleateInstraction(InstractionType.set, tempBlock);
        }
    }

    ///<Summary>
    ///TempBlockを削除するときは必ずこのメソッドを使って欲しい。
    ///<Summary>
    public bool DeleteTempBlock(GameObject obj)
    {
        DeleteSetBlockInstraction(obj);
        bool isSuccess = tempblocks.Remove(obj);
        Destroy(obj);
        return isSuccess;

        void DeleteSetBlockInstraction(GameObject tempBlock)
        {
            List<Instraction> instractions = InstractionManager.Instance.SearchInstraction(InstractionType.set, tempBlock);
            foreach(Instraction instraction in instractions)
            {
                instraction.state = InstractionState.finished;
            }
        }
    }
}
