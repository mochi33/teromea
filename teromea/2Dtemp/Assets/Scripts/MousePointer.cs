using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MousePointer : SingletonMonoBehaviour<MousePointer>
{
    public GameObject[] nearBlock = new GameObject[100];
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

<<<<<<< HEAD
    private Vector3 prevPos;
/*private void OnMouseDown()
{
    prevPos = Input.mousePosition;
}

private void OnMouseUp()
{
    Vector2 set = (prevPos + Input.mousePosition)/2;
    Vector2 range = new Vector2(prevPos.x-Input.mousePosition.x,prevPos.y-Input.mousePosition.y);
    Collider2D[] pointcol = Physics2D.OverlapBoxAll(set, range, 0);
    foreach(Collider2D col in pointcol)
    {
        if(col.CompareTag("Block"))
        {
         col.GetComponent<Block>().Set();   
        }
    }    
}
*/
    private void searchNearObjects()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, Model.BLOCK_SIZE, LayerMask.GetMask("Block"));
=======
    private void searchNearObjects () {
        //レイヤーの指定https://www.ame-name.com/archives/5621
        LayerMask layerMask = Model.BLOCK_LAYER | Model.TEMPBLOCK_LAYER | Model.LADDER_LAYER;
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, Model.BLOCK_SIZE, layerMask);
>>>>>>> origin/HashimotoNaoki
        blocklength = cols.Length;
        for (int i = 0; i < cols.Length; i++)
        {
            nearBlock[i] = cols[i].gameObject;
        }
    }

    private void initializeNearBlock()
    {
        for (int i = 0; i < 9; i++)
        {
            nearBlock[i] = null;
        }
    }
}
