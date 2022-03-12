using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMovePoint : SingletonMonoBehaviour<SetMovePoint>
{
    private GameObject underBlock = null;
    public GameObject moveTargetPrefab;
    private List<GameObject> moveTargetObjList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(SelectObject.Instance.selectedHuman != null)
        {
            if(Input.GetMouseButtonDown(1))
            {
                Vector2 mousePosition = MousePointer.Instance.transform.position;
                underBlock = null;
                if(!PhysicsFunc.isThereAnyObjectOnThePoint(mousePosition, Model.BLOCK_LAYER | Model.TEMPBLOCK_LAYER | Model.HUMAN_LAYER | Model.MOVETARGET_LAYER)
                && (underBlock = Physics2D.OverlapPoint(mousePosition + Vector2.down * Model.BLOCK_SIZE, Model.BLOCK_LAYER)?.gameObject) != null)
                {
                    CreateMoveTarget(new Vector2(mousePosition.x, underBlock.transform.position.y + (Model.BLOCK_SIZE + Model.HUMAN_SIZE.HEIGHT) / 2), SelectObject.Instance.selectedHuman);
                }
            }
        }
        
    }

    private void CreateMoveTarget(Vector2 pos, Human executer)
    {
        GameObject target = Instantiate(moveTargetPrefab, pos, Quaternion.identity);
        moveTargetObjList.Add(target);
        SetMoveInstraction(target);
        
        void SetMoveInstraction(GameObject target)
        {
            Instraction instraction = InstractionManager.Instance.CleateInstraction(InstractionType.move, target);
            SetInstraction.Instance.SetInstractionToHuman(instraction, executer);
        }
    }

    public void DeleteMoveTarget(GameObject moveTarget)
    {
        if(moveTargetObjList.Remove(moveTarget))
        {
            DeleteMoveInstraction(moveTarget);
            Destroy(moveTarget);
        }

        void DeleteMoveInstraction(GameObject moveTarget)
        {
            List<Instraction> list = InstractionManager.Instance.SearchInstraction(InstractionType.move, moveTarget);
            foreach(Instraction instraction in list)
            {
                instraction.state = InstractionState.finished;
            }
        }
    }
}
