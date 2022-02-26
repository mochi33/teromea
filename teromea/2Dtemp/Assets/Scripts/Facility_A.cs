using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Facility_A : Token
{
    //ここにアタッチしたオブジェクトの情報が入る
    private Rigidbody2D rig;
    //射程距離
    public float range;
    // ショットの速度
    const float SHOT_SPEED = 5.0f;
    public GameObject shot;
    public float speed;
    public int interval = 0;

    Collider2D enemy;


    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy == null)
        {
            //敵の情報持ってないなら
            enemy = Search();
            //探してもいないなら終わる
            if (enemy == null)
            {
                interval=100;
                return;
            }
        }
        //Collider2Dの座標が分かればいい
        var dx = this.X - enemy.transform.position.x;
        var dy = this.Y - enemy.transform.position.y;
        // 敵への角度を取得(⑥)
        float targetAngle = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg+180;
        // 現在向いている角度との差を求める
        float dAngle = Mathf.DeltaAngle(Angle, targetAngle);
        // 差の0.2だけ回転する
        Angle += dAngle * 0.2f;
        // もう一度角度差を求める(⑦)
        float dAngle2 = Mathf.DeltaAngle(Angle, targetAngle);
        if (Mathf.Abs(dAngle2) > 16)
        {
            // 角度が大きい(16度より大きい)場合は撃てない
            return;
        }
        if(interval<=0){
        //矢を生成(なんか親子関係使った方が綺麗っぽい)
        Instantiate(shot,new Vector3(this.X,this.Y,0),Quaternion.Euler(0,0,Angle));
        interval=100;
        }else{
            interval=interval-1;
        }
    }
    //Collider:当たり判定に使う図形
    Collider2D Search()
    {
        //範囲内のオブジェクトを取得
        Collider2D[] pointcol = Physics2D.OverlapCircleAll(rig.position, range);
        foreach (Collider2D col in pointcol)
        {
            if (col.gameObject.CompareTag("Enemy"))
            {
                //Enemyタグの一つを返す
                return col;
            }
        }
        return null;
    }
    public static Type CreateInstance_A<Type> (GameObject prefab, Vector3 p, float direction = 0.0f, float speed = 0.0f) where Type : Token
  {
    GameObject g = Instantiate (prefab, p, Quaternion.identity) as GameObject;
    Type obj = g.GetComponent<Type> ();
    if(obj.RigidBody)
    {
      // RigidBodyを登録している時だけ設定
      obj.SetVelocity (direction, speed);
    }
    return obj;
  }
}
