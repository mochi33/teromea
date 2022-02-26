using UnityEngine;
using System.Collections;

/// ショット
public class Shot : Token
{
    // ショットオブジェクト管理
    public static TokenMgr<Shot> parent;
    public GameObject shot;
    public float speed;
    public int power;
    // ショットを撃つ
    private Rigidbody2D myRigid;
    // Start is called before the first frame update
    void Start()
    {
        myRigid = this.GetComponent<Rigidbody2D>();
        myRigid.AddForce(transform.right*speed);
    }
    void Update()
    {
        if (IsOutside())
        {
            // 画面外に出たので消滅,逆に言えば画面外では動いてくれない
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if(collisionInfo.gameObject.CompareTag("Enemy")){
            collisionInfo.gameObject.GetComponent<Enemy>().Hpdown(power);
            Destroy(this.gameObject);
        }
    }

    /// 消滅 (①)
    public override void Vanish()
    {
        // パーティクル生成
        for (int i = 0; i < 4; i++)
        {
            int timer = Random.Range(20, 40);
            // 反対方向に飛ばす (②)
            float dir = Direction - 180 + Random.Range(-60, 60);
            float spd = Random.Range(1.0f, 1.5f);
            Particle p = Particle.Add(Particle.eType.Ball, timer, X, Y, dir, spd);
            if (p)
            {
                // 小さくする
                p.Scale = 0.6f;
                // 赤色にする
                p.SetColor(1, 0.0f, 0.0f);
            }
        }
        // 親クラスの消滅処理を呼び出す
        base.Vanish();
    }
}
