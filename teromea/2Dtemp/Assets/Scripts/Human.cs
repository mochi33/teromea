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
                StopActionCoroutines();
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
        if(currentCouroutine != null)
        {
            StopCoroutine(currentCouroutine);
        }
    }

    public IEnumerator MoveToTarget(Instraction instraction)
    {
        yield return StartCoroutine(charmove.MoveToPosition(instraction.target.transform.position));
        InstractionManager.Instance.DeleteInstraction(instraction);
        yield break;
    }

    public IEnumerator Attack(Instraction instraction)
    {
        InstractionManager.Instance.DeleteInstraction(instraction);
        yield break;
    }

    public IEnumerator SetBlock(Instraction instraction)
    {
        yield return StartCoroutine(charmove.MoveToTarget(instraction.target));
        do {
        yield return new WaitForSeconds(1.0f);
        } while(!ConvertTempBlockIntoBlock(instraction.target));

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
        InstractionManager.Instance.DeleteInstraction(instraction);
        yield break;
    }

}
