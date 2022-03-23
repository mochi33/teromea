using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObject : SingletonMonoBehaviour<SelectObject>
{
    public GameObject selectedObject = null;

    public LayerMask layerMask = 1 << 6 | 1 << 7 | 1 << 8;

    public Human selectedHuman = null;

    public Block selectedBlock = null;

    public TempBlock selectedTempBlock = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(selectedHuman != null)
            {
                selectedHuman.isSelected = false;
                selectedHuman = null;
            }
                
            if(selectedBlock != null)
            {
                selectedBlock.isSelected = false;
                selectedBlock = null;
            }
                
            if(selectedTempBlock != null)
            {
                selectedTempBlock.isSelected = false;
                selectedTempBlock = null;
            }
                
            selectedObject = null;
        

            GameObject anySelectableObj = Physics2D.OverlapPoint(MousePointer.Instance.transform.position, layerMask: layerMask)?.gameObject;
            
            if(anySelectableObj != null)
            {
                selectedObject = anySelectableObj;
                switch(anySelectableObj.tag)
                {
                    case "Human":
                    selectedHuman = anySelectableObj.GetComponent<Human>();
                    selectedHuman.isSelected = true;
                    break;

                    case "TempBlock":
                    selectedTempBlock = anySelectableObj.GetComponent<TempBlock>();
                    selectedTempBlock.isSelected = true;
                    break;

                    case "Block":
                    selectedBlock = anySelectableObj.GetComponent<Block>();
                    selectedBlock.isSelected = true;
                    break;
                }
            }
        }
    }
}
