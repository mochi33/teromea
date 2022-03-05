using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    //https://www.youtube.com/watch?v=8FLg8lU1jBY
    //https://nn-hokuson.hatenablog.com/entry/2020/10/27/151858#File%E3%82%AF%E3%83%A9%E3%82%B9%E3%82%92%E4%BD%BF%E3%81%A3%E3%81%A6%E8%AA%AD%E3%81%BF%E6%9B%B8%E3%81%8D%E3%81%99%E3%82%8B
    //https://kurokumasoft.com/2022/01/03/unity-savesystem-using-json/
    //https://logicalbeat.jp/blog/8195/
    //クローン装置の座標
    Vector2 rig = new Vector2(0, 0);
    //セーブする範囲
    Vector2 range = new Vector2(32, 32);
    //ここにオブジェクトの位置と名前突っ込む
    [System.Serializable]
    public class ObjData
    {
        public Vector2 pos;
        public string name;
    }



    void Start()
    {
        //rig(HumanGener)の座標は(0，0)にしとく
        //rig = GameObject.Find("HumanGener").transform.position
        Save();
        Load();
    }
    public void Save()
    {
        Collider2D[] pointcol = Physics2D.OverlapBoxAll(rig, range, 0);
        //ファイルの初期化(書き込みバグったらデータ全ロスするから気をつけて♪)
        File.WriteAllText(Application.dataPath + "/save.json", null);
        foreach (Collider2D col in pointcol)
        {
            ObjData rig = new ObjData();
            //それぞれのオブジェクトの位置と名前を記録
             rig.pos = col.gameObject.transform.position;
             rig.name = col.gameObject.name;
            //Debug.Log(josn+name);
            File.AppendAllText(Application.dataPath + "/save.json", JsonUtility.ToJson(rig)+"\n");
            //File.WriteAllText(Application.dataPath+"/save.txt",josn+name);
        }
    }

    public void Load()
    {
        Debug.Log("OK");
        // 一行ずつ読み込む
        using (var fs = new StreamReader(Application.dataPath + "/save.json", System.Text.Encoding.GetEncoding("UTF-8")))
        {
            while (fs.Peek() != -1)
            {
                ObjData gir = new ObjData();
                gir = JsonUtility.FromJson<ObjData>(fs.ReadLine());
                /*
                ここで各オブジェクトの生成をすればいい
                */
                Debug.Log(gir.name);
            }

        }
    }
}
