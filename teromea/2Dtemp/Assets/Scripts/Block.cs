using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{

    public BlockType type;

    // Start is called before the first frame update
    void Start()
    {
        transform.parent = GameObject.Find ("BlockManager").transform;
        switch(gameObject.name)
        {
            case "Dirt":
                type = BlockType.dirt;
                break;
            
            case "Stone":
                type = BlockType.stone;
                break;

            default:
                break;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
