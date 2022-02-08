using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBlock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.parent = GameObject.Find ("TempBlockManager").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
