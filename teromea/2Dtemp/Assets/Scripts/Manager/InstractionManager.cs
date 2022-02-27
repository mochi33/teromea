using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InstractionType
{
    move,
    dig,
    set,
    attack,

}

public enum InstractionState
{
    waiting,
    inProcess,
    finished,
    cancel,
}

public struct Instraction
{
    public InstractionType type;
    public GameObject target;

    public InstractionState state;

    public Instraction(InstractionType type, GameObject target)
    {
        this.type = type;
        this.target = target;
        this.state = InstractionState.waiting;
    }

}
public class InstractionManager : SingletonMonoBehaviour<InstractionManager>
{

    public List<Instraction> instractionList = new List<Instraction>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddInstraction(Instraction instraction)
    {
        if (instractionList.Count <= Model.MAX_INSTRACTION)
        {
            instractionList.Add(instraction);
        }
    }

    public void RemoveInstraction(Instraction instraction)
    {
        instractionList.Remove(instraction);
    }

}
