using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBlock : MonoBehaviour
{
    BlockType blockType;
    public bool isSelected = false;
    // Start is called before the first frame update
    void Start()
    {
        transform.parent = GameObject.Find ("TempBlockManager").transform;
        blockType = UIManager.Instance.selectedBlockType; //この文は仮で、本当はUIから選択しているブロックを持ってきて代入したい
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    ///<Summary>
    ///このインスタンスのTempBlockオブジェクトをブロック化する
    ///<Summary>
    public bool ConvertThisIntoBlock()
    {
        if(BlockManager.Instance.CleateBlock(transform.position, blockType))
        {
            if(TempBlockManager.Instance.DeleteTempBlock(gameObject))
            {
                return true;
            }
        }
        
        return false;
    }
}
