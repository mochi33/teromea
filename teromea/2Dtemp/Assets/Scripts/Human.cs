using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{    
    public CharacterMove charmove;

    public Instraction myInstraction = new Instraction(InstractionType.noInstraction, null);

    private bool isInstractionChanged = false;
    private Coroutine currentCouroutine = null;

    // Start is called before the first frame update

    void Start()
    {
        transform.parent = GameObject.Find ("HumanManager").transform;
        charmove = GetComponent<CharacterMove>();
        StartCoroutine(ExecuteInstraction());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReceiveInstraction(Instraction instraction)
    {
        Debug.Log("Receive Instraction");
        StopActionCoroutines();
        instraction.state = InstractionState.inProcess;
        instraction.executer = this;
        myInstraction = instraction;
        isInstractionChanged = true;
        
    }

    public IEnumerator ExecuteInstraction()
    {
        while(true)
        {
            if(isInstractionChanged)
            {
                isInstractionChanged = false;
                switch (myInstraction.type)
                {
                    case InstractionType.move:
                    currentCouroutine = StartCoroutine(MoveToTarget(myInstraction));
                    break;

                    case InstractionType.attack:
                    currentCouroutine = StartCoroutine(Attack(myInstraction));
                    break;

                    case InstractionType.dig:
                    currentCouroutine = StartCoroutine(Dig(myInstraction));
                    break;

                    case InstractionType.set:
                    currentCouroutine = StartCoroutine(SetBlock(myInstraction));
                    break;

                    default:
                    break;
                }
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    public void StopActionCoroutines()
    {
        charmove.StopAllCoroutines();
        charmove.StopWalk();
        if(currentCouroutine != null)
        {
            StopCoroutine(currentCouroutine);
        }
    }

    public void FinishInstraction(Instraction instraction)
    {
        instraction.state = InstractionState.finished;
        StopActionCoroutines();
    }

    public IEnumerator MoveToTarget(Instraction instraction)
    {
        yield return StartCoroutine(charmove.MoveToPosition(instraction.target.transform.position));
        FinishInstraction(instraction);
        yield break;
    }

    public IEnumerator Attack(Instraction instraction)
    {
        FinishInstraction(instraction);
        yield break;
    }

    public IEnumerator SetBlock(Instraction instraction)
    {
        yield return StartCoroutine(charmove.MoveToTarget(instraction.target));
        do
        {
            yield return new WaitForSeconds(1.0f);
        } 
        while(!ConvertTempBlockIntoBlock(instraction.target));
        FinishInstraction(instraction);
        yield break;

        bool ConvertTempBlockIntoBlock(GameObject tempBlock)
        {
            if(tempBlock.GetComponent<TempBlock>()?.ConvertThisIntoBlock() == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public IEnumerator Dig(Instraction instraction)
    {
        Block block = instraction.target.GetComponent<Block>();
        yield return StartCoroutine(charmove.MoveToTarget(instraction.target));
        do
        {
            yield return new WaitForSeconds(1.0f);
            block.hp -= 5.0f;
        } 
        while(block.hp > 0f);
        FinishInstraction(instraction);
        yield break;
    }

}
