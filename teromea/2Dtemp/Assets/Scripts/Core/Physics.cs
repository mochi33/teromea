using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhysicsFunc
{
    // Start is called before the first frame update
    public static bool isThereAnyObjectOnThePoint(Vector2 pos, LayerMask layerMask)
    {
        Collider2D[] pointcols = Physics2D.OverlapPointAll(pos, layerMask: layerMask);
        if(pointcols.Length > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }
}
