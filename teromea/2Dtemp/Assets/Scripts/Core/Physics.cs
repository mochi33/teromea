using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhysicsFunc
{
    // Start is called before the first frame update
    public static bool isThereAnyObjectOnThePoint(LayerMask layerMask)
    {
        Collider2D[] pointcols = Physics2D.OverlapPointAll(MousePointer.Instance.transform.position, layerMask: layerMask);
        if(pointcols.Length > 0)
        {
            return true;
        } else {
            return false;
        }
        
    }
}
