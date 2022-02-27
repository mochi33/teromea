using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject testObj;
    void Start()
    {
        InstractionManager.Instance.AddInstraction(new Instraction(InstractionType.move, testObj));
        SetInstraction.Instance.AssignInstraction();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
