using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BlockType
{
    dirt,
    stone,

}
public class BlockManager : SingletonMonoBehaviour<TempBlockManager>
{

    public List<GameObject> blocks = new List<GameObject>();
    public GameObject prefabobj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CleateBlock (Vector3 pos, BlockType blockType) 
    {
        if(!PhysicsFunc.isThereAnyObjectOnThePoint(1 << 6 | 1 << 7))
        {
            blocks.Add(Instantiate(prefabobj, pos, Quaternion.identity));
            return true;
        } 
        else 
        {
            return false;
        }
    }

    public bool DeleteBlock(GameObject obj)
    {
        Destroy(obj);
        return blocks.Remove(obj);
    }
}
