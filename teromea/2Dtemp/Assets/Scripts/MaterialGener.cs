using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialGener : MonoBehaviour
{
    public GameObject[] Materials;
    float interval = 1.0f;
    float timer = 2;



    // Update is called once per frame
    void Update()//適当な位置に複製
    {
        //https://www.youtube.com/watch?v=QuV3ktzUDmE
        //https://masavlog.com/programming/tetris/unity-tetris-5/
        this.timer += Time.deltaTime;
        if (this.interval < this.timer)
        {
            int x = Random.Range(-10, 10);
            //縦には出来ないようにする(最終的には下にブロックがあるときだけしたい)
            //int y = Random.Range(-10, 10);
            //すでにブロックが存在していないかチェック
            bool isputable = true;
            Collider2D[] pointcol = Physics2D.OverlapPointAll(new Vector3(x,0,0));
            foreach (Collider2D col in pointcol)
            {
                if (col.gameObject.CompareTag("Block"))
                {
                    isputable = false;
                    Debug.Log("false");
                }
            }
            //ここまで

            if (isputable)
            {
                this.timer = 0;
                this.interval = Random.Range(0.0f, 2.0f);
                //GameObject Materials_instance = Instantiate(Materials[Random.Range(0,Materials.Length)]) as GameObject;
                TempBlockManager.Instance.CleateTempBlock(new Vector3(x, 0, 0));                

            //Materials_instance.transform.position=new Vector3(x,y,0);
            }
        }
    }
    /*private void OnCollisionEnter2D(Collision2D collision)//collision=当たってきたやつ
    {
        //何が当たっても消える
        Destroy(this.gameObject);
    }*/
}
