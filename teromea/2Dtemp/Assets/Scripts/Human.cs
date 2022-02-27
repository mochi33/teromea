using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{    
    public CharacterMove charmove;

    public Instraction myInstraction;

    private bool isInstractionChanged = false;

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
        myInstraction = instraction;
        isInstractionChanged = true;
    }

    public IEnumerator ExecuteInstraction()
    {
        while(true)
        {
            if(isInstractionChanged)
            {
                switch (myInstraction.type)
                {
                    case InstractionType.move:
                    StartCoroutine(MoveToTarget(myInstraction));
                    break;

                    case InstractionType.attack:
                    StartCoroutine(Attack(myInstraction));
                    break;

                    case InstractionType.dig:
                    StartCoroutine(Dig(myInstraction));
                    break;

                    case InstractionType.set:
                    StartCoroutine(SetBlock(myInstraction));
                    break;

                    default:
                    break;
                }
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    public IEnumerator MoveToTarget(Instraction instraction)
    {
        StartCoroutine(charmove.MoveToPosition(instraction.target.transform.position));
        yield return null;
    }

    public IEnumerator Attack(Instraction instraction)
    {
        yield return null;
    }

    public IEnumerator SetBlock(Instraction instraction)
    {
        yield return null;
    }

    public IEnumerator Dig(Instraction instraction)
    {
        yield return null;
    }

}
