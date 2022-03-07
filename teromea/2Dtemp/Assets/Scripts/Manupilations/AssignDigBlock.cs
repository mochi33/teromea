using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignDigBlock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(UIManager.Instance.selectedManipulation == Manipulation.assignDigBlock)
        {
            if(Input.GetMouseButton(0))
            {
                GameObject blockObj = Physics2D.OverlapPoint(MousePointer.Instance.transform.position, layerMask: 1 << 6)?.gameObject;
                Block block = blockObj?.GetComponent<Block>();
                if(block?.isDig == false)
                {
                    block.isDig = true;
                    InstractionManager.Instance.CleateInstraction(InstractionType.dig, blockObj);
                }
            }
        }
    }

}
