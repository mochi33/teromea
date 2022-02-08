using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Feature
{
    human,
    enemy,
}

public class Info
{
    int hp;
    int speed;
}

public abstract class Character : MonoBehaviour
{
    private string name;
    private Feature feature;
    private Info info;

    public Character(string name, Feature feature, Info info) 
    {
        this.name = name;
        this.feature = feature;
        this.info = info;
    }

}
