using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointer : SingletonMonoBehaviour<MousePointer>
{
    public GameObject[] nearBlock = new GameObject[9];
    public int blocklength = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0.0f;
        transform.position = pos;
        initializeNearBlock();//nearBlock配列を初期化
        searchNearObjects();//マウスポインタ付近にあるBlockオブジェクトを取得
    }

    private void searchNearObjects () {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, Model.BLOCK_SIZE, LayerMask.GetMask("Block"));
        blocklength = cols.Length;
        for (int i = 0; i < cols.Length; i++)
        {
            nearBlock[i] = cols[i].gameObject;
        }
    }

    private void initializeNearBlock () {
        for (int i = 0; i < 9; i++) 
        {
            nearBlock[i] = null;
        }
    }
}