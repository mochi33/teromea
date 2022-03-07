using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BlockType
{
    dirt,
    stone,

}
public class BlockManager : SingletonMonoBehaviour<BlockManager>
{

    public List<GameObject> blocks = new List<GameObject>();
    public GameObject prefabDirt;
    public GameObject prefabStone;
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
        if(!PhysicsFunc.isThereAnyObjectOnThePoint(pos, Model.BLOCK_LAYER))
        {
            switch(blockType)
            {
                case BlockType.dirt:
                blocks.Add(Instantiate(prefabDirt, pos, Quaternion.identity));
                break;

                case BlockType.stone:
                blocks.Add(Instantiate(prefabStone, pos, Quaternion.identity));
                break;

                default:
                return false;

            }
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
