using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
        List<Instraction> tempList = new List<Instraction>(instractionList);
        foreach(Instraction instraction in tempList)
        {
            if(instraction.state == InstractionState.finished)
            {
                DeleteInstraction(instraction);
            }
            else if(instraction.state == InstractionState.cancel)
            {
                DeleteInstraction(instraction);
            }
        }
    }

    public Instraction CleateInstraction(InstractionType type, GameObject target)
    {
        Instraction instraction = new Instraction(type, target);
        AddInstraction(instraction);
        return instraction;
    }

    private void DeleteInstraction(Instraction instraction)
    {
        //空の命令を送信
        instraction.executer?.ReceiveInstraction(CleateInstraction(InstractionType.noInstraction, null));       
        RemoveInstraction(instraction);
    }

    public List<Instraction> SearchInstraction(InstractionType type = InstractionType.noInstraction, GameObject target = null)
    {
        List<Instraction> searchList = new List<Instraction>(instractionList);
        List<Instraction> searchedList = new List<Instraction>();

        if(type != InstractionType.noInstraction)
        {
            foreach(Instraction instraction in searchList)
            {
                if(instraction.type == type)
                {
                    searchedList.Add(instraction);
                }
            }
            searchList = new List<Instraction>(searchedList);
        }

        if(target != null)
        {
            searchedList.Clear();
            foreach(Instraction instraction in searchList)
            {
                if(instraction.target == target)
                {
                    searchedList.Add(instraction);
                }
            }
        }

        return searchedList;
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

public class Instraction
{
    public InstractionType type;
    public GameObject target;

    public InstractionState state;

    public Human executer;

    public Instraction(InstractionType type, GameObject target)
    {
        this.type = type;
        this.target = target;
        this.state = InstractionState.waiting;
        this.executer = null;
    }

}

public enum InstractionType
{
    move,
    dig,
    set,
    attack,
    noInstraction

}

public enum InstractionState
{
    waiting,
    inProcess,
    finished,
    cancel,
}
