using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
     //位置を引数として渡せるようにしたい
    //すでにブロックが存在していないかチェック
            bool isputable = true;
            Collider2D[] pointcol = Physics2D.OverlapPointAll(MousePointer.Instance.transform.position); 
            foreach(Collider2D col in pointcol)
            {
                if (col.gameObject.CompareTag("Block"))
                {
                    isputable = false;
                    Debug.Log("false");
                }
            }
            //ここまで   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
