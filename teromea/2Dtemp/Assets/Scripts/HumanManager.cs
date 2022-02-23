using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanManager : SingletonMonoBehaviour<HumanManager>
{

    public GameObject[] Human = new GameObject[Model.MAX_HUMAN];
    public int humanlength;
    public GameObject prefabobj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CleateHuman (Vector3 pos) {
        if (humanlength < Model.MAX_HUMAN)
            Human[humanlength - 1] = Instantiate(prefabobj, pos, Quaternion.identity);
    }
}
