using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetInstraction : SingletonMonoBehaviour<SetInstraction>
{

    private HumanManager humanManager;
    private InstractionManager instractionManager;
    // Start is called before the first frame update
    void Start()
    {
        humanManager = HumanManager.Instance;
        instractionManager = InstractionManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //命令に割り当てる人を決定し実行させる、とりあえず仮で命令対象に一番近い人に命令を割り当てるようにした
    public void AssignInstraction()
    {
        Debug.Log("StartAssignInstraction");
        foreach(Instraction instraction in instractionManager.instractionList)
        {
            if(instraction.state == InstractionState.waiting && !(instraction.type == InstractionType.noInstraction || instraction.type == InstractionType.move))
            {
                //上のifが機能していない
                float minDistance = Model.MAX_INSTRACTION_RANGE;
                Human minHuman = null;
                foreach(Human human in humanManager.humanList)
                {
                    if(human.myInstraction?.type == InstractionType.noInstraction)
                    {
                        //上のifが機能していない
                        float distance = Vector2.Distance(human.gameObject.transform.position, instraction.target.transform.position);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            minHuman = human;
                        }
                    }
                }
                if(minHuman != null)
                {
                    SetInstractionToHuman(instraction, minHuman);
                }
            }
        }
    }

    ///<Summary>
    ///人に次の命令をセットし、現在の命令を終了させる。
    ///<Summary>
    public void SetInstractionToHuman(Instraction instraction, Human human)
    {
        Debug.Log("SetInstractiion");

        Instraction currentInstraction;
        if((currentInstraction = human?.myInstraction) != null)
        {
            if(currentInstraction.nextInstraction == null)
            {
                currentInstraction.nextInstraction = instraction;
                currentInstraction.state = InstractionState.finished;
            }
        }
        
    }

    ///<Summary>
    ///人に次の命令セットし、現在の命令を一時停止させる。
    ///<Summary>
    public void SetInterruptInstraction(Instraction instraction, Human human)
    {
        Debug.Log("SetInstractiion");

        Instraction currentInstraction;
        if((currentInstraction = human?.myInstraction) != null)
        {
            if(currentInstraction.nextInstraction == null)
            {
                currentInstraction.nextInstraction = instraction;
                if(currentInstraction.type == InstractionType.noInstraction)
                {
                    currentInstraction.state = InstractionState.cancel;
                }
                else
                {
                    currentInstraction.state = InstractionState.finished;
                }
            }
        }
    }

}
