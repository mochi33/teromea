using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{

    public BlockType type;
    public float hp;
    public bool isDig = false;
    public bool isSelected = false;
    public SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        transform.parent = GameObject.Find ("BlockManager").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        switch(gameObject.name)
        {
            case "Dirt":
                type = BlockType.dirt;
                hp = 10f;
                break;
            
            case "Stone":
                type = BlockType.stone;
                hp = 20f;
                break;

            default:
                break;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if(hp < 0f)
        {
            BlockManager.Instance.DeleteBlock(gameObject);
        }

        if(isDig)
        {
            spriteRenderer.color = new Color32(140, 255, 140, 255);
        }
        else
        {
            spriteRenderer.color = new Color32(255, 255, 255, 255);
        }
    }

}
