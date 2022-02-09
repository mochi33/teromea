using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //こいつの役割はゲームスタート→ゲーム中→ゲームオーバと判定すること
    //https://qiita.com/tsukasa_wear_parker/items/09d4bcc5af3556b9bb3a
    GameObject HGhpbar;//HGhpbar全部突っ込む
    HGhpbar script;//HGhpbarのスクリプトを突っ込む
    public GameObject gameOverUI;
    // Start is called before the first frame update
    void Start()
    {
        //まずHGhpbarの情報全部持ってくる
        HGhpbar = GameObject.Find("HGhpbar");
        //そっからscriptにHGhpbarのスクリプトを突っ込む
        script=HGhpbar.GetComponent<HGhpbar>();
        //これでscriptに入ったんで取り出せるはず←GetComponentの後ろには()いるって
    }

    // Update is called once per frame
    void Update()
    {
        if(script.currentHp==0){//HumanGenerのHpを聞いて、0ならば
            GameOver();
        }
    }

    public void GameOver()
    {
        Debug.Log("ゲームオーバ");
        //UIを見えるようにしている
        gameOverUI.SetActive(true);
    }

    public void GameSet()
    {
        //”New”という名前のsceneを読み込んでいる
        SceneManager.LoadScene("New");
        //gameOverUI.SetActive(false);
    }
}
