using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AssignInstraction());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator AssignInstraction()
    {
        while(true)
        {
            StartCoroutine(SetInstraction.Instance.AssignInstraction());
            yield return new WaitForSeconds(2.0f);
        }
    }
}
