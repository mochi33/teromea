using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genereter : MonoBehaviour
{
    public GameObject[] Materials_prefab;
    float interval =1.0f;
    float timer =2;

    

    // Update is called once per frame
    void Update()//適当な位置に複製
    {
        //https://www.youtube.com/watch?v=QuV3ktzUDmE
        //https://masavlog.com/programming/tetris/unity-tetris-5/
        this.timer+=Time.deltaTime;
        if(this.interval<this.timer)
        {
            this.timer=0;
            this.interval=Random.Range(0.0f,2.0f);
            GameObject Materials_instance = Instantiate(Materials_prefab[Random.Range(0,Materials_prefab.Length)]) as GameObject;
            float x = Random.Range(-10.0f,10.0f);
            float y = Random.Range(-10.0f,10.0f);
            Materials_instance.transform.position=new Vector3(x,y,0);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)//collision=当たってきたやつ
    {
        //何が当たっても消える
        Destroy(this.gameObject);
    }

}
