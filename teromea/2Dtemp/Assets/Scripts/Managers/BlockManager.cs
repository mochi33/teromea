using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BlockType
{
    dirt,
    stone,
    ladder,

}
public class BlockManager : SingletonMonoBehaviour<BlockManager>
{
    
    public bool[,] blockMap = new bool[(int)(Model.WORLD_WIDTH / Model.BLOCK_SIZE), (int)(Model.WORLD_HEIGHT / Model.BLOCK_SIZE)];

    public bool[,] ladderMap = new bool[(int)(Model.WORLD_WIDTH / Model.BLOCK_SIZE), (int)(Model.WORLD_HEIGHT / Model.BLOCK_SIZE)];
    public List<GameObject> blocks = new List<GameObject>();
    public GameObject prefabDirt;
    public GameObject prefabStone;
    public GameObject prefabLadder;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < (int)(Model.WORLD_HEIGHT / Model.BLOCK_SIZE); i++)
        {
            for(int h = 0; h < (int)(Model.WORLD_WIDTH / Model.BLOCK_SIZE); h++)
            {
                blockMap[h, i] = false;
                ladderMap[h, i] = false;
            }
        }

        List<GameObject> children = new List<GameObject>();
        for(int i = 0; i < transform.childCount; i++)
        {
            children.Add(transform.GetChild(i).gameObject);
        }

        foreach(GameObject blockObj in children)
        {
            Vector2Int worldPos = new Vector2Int();
            worldPos = World.GetWorldPosition(blockObj.transform.position);
            if(blockObj.CompareTag("Ladder"))
            {
                ladderMap[worldPos.x, worldPos.y] = true;
            }
            else
            {
                blockMap[worldPos.x, worldPos.y] = true;
            }
            blocks.Add(blockObj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CleateBlock (Vector3 pos, BlockType blockType) 
    {
        if(!PhysicsFunc.isThereAnyObjectOnThePoint(pos, Model.BLOCK_LAYER | Model.LADDER_LAYER)
        && World.isOnWorld(pos))
        {
            GameObject blockObj = null;
            switch(blockType)
            {
                case BlockType.dirt:
                blocks.Add(blockObj = Instantiate(prefabDirt, pos, Quaternion.identity));
                AddBlockMap(blockObj);
                break;

                case BlockType.stone:
                blocks.Add(blockObj = Instantiate(prefabStone, pos, Quaternion.identity));
                AddBlockMap(blockObj);
                break;

                case BlockType.ladder:
                blocks.Add(blockObj = Instantiate(prefabLadder, pos, Quaternion.identity));
                AddLadderMap(blockObj);
                break;

                default:
                return false;

            }
            Block block = blockObj?.GetComponent<Block>();
            if(block != null)
            {
                block.type = blockType;
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
        if(obj != null)
        {
            if(obj.CompareTag("Ladder"))
            {
                RemoveLadderMap(obj);
            }
            else
            {
                RemoveBlockMap(obj);
            }
            Destroy(obj);
            blocks.Remove(obj);
            return true;
        }
        else
        {
            return  false;
        }
    }

    private bool AddBlockMap(GameObject blockObj)
    {
        if(blockObj != null)
        {
            Vector2Int worldPos = new Vector2Int();
            worldPos = World.GetWorldPosition(blockObj.transform.position);
            blockMap[worldPos.x, worldPos.y] = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool RemoveBlockMap(GameObject blockObj)
    {
        if(blockObj != null)
        {
            Vector2Int worldPos = new Vector2Int();
            worldPos = World.GetWorldPosition(blockObj.transform.position);
            blockMap[worldPos.x, worldPos.y] = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool AddLadderMap(GameObject blockObj)
    {
        if(blockObj != null)
        {
            Vector2Int worldPos = new Vector2Int();
            worldPos = World.GetWorldPosition(blockObj.transform.position);
            ladderMap[worldPos.x, worldPos.y] = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool RemoveLadderMap(GameObject blockObj)
    {
        if(blockObj != null)
        {
            Vector2Int worldPos = new Vector2Int();
            worldPos = World.GetWorldPosition(blockObj.transform.position);
            ladderMap[worldPos.x, worldPos.y] = false;
            return true;
        }
        else
        {
            return false;
        }
    }
}
