using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTempBlock : SingletonMonoBehaviour<TempBlockManager>
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            for(int i = 0; i < MousePointer.Instance.blocklength; i++)
            {
                float x = MousePointer.Instance.transform.position.x;
                float y = MousePointer.Instance.transform.position.y;
                float objx = MousePointer.Instance.nearBlock[i].transform.position.x;
                float objy = MousePointer.Instance.nearBlock[i].transform.position.y;
                bool ablex = false;
                bool abley = false;

                if(Mathf.Abs(objx - x) < Model.BLOCK_SIZE * 1.5f && Mathf.Abs(objx - x) > Model.BLOCK_SIZE * 0.5f) 
                {
                    ablex = true;
                }

                if(Mathf.Abs(objy - y) < Model.BLOCK_SIZE * 1.5f && Mathf.Abs(objy - y) > Model.BLOCK_SIZE * 0.5f) 
                {
                    abley = true;
                }

                if(!(ablex && abley))
                {
                    if (ablex)
                    {
                       if(x > objx)
                        {
                            TempBlockManager.Instance.CleateTempBlock(new Vector3(objx + Model.BLOCK_SIZE, objy, 0));
                        } 
                        else 
                        {
                            TempBlockManager.Instance.CleateTempBlock(new Vector3(objx - Model.BLOCK_SIZE, objy, 0));
                        }
                            break;
                        }

                        if(abley)
                        {
                            if(y > objy)
                            {
                                TempBlockManager.Instance.CleateTempBlock(new Vector3(objx, objy + Model.BLOCK_SIZE, 0));
                            } 
                            else 
                            {
                                TempBlockManager.Instance.CleateTempBlock(new Vector3(objx, objy - Model.BLOCK_SIZE, 0));
                            }
                            break;
                        }
                    }
                }
        }
        
    }
}
