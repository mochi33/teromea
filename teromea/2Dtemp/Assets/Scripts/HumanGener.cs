using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanGener : MonoBehaviour
{
    public GameObject[] Materials;
    float interval = 1.0f;
    float timer = 2;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.timer += Time.deltaTime;
        if (this.interval < this.timer)
        {
            //https://qiita.com/haifuri/items/cec28c764eebbc9bae0b
            //HumanGenerの座標と人を生み出す位置を連動したい
            Vector3 tmp = GameObject.Find("HumanGener").transform.position;
            this.timer = 0;
            this.interval = Random.Range(0.0f, 2.0f);
            //GameObject Materials_instance = Instantiate(Materials[Random.Range(0,Materials.Length)]) as GameObject;
            HumanManager.Instance.CleateHuman(tmp);
        }
    }
}
