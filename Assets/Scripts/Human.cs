using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterFeature
{
    human,
    enemy,
}

public class CharacterInfo
{
    float hp;
    float speed;
}

public abstract class Character : MonoBehaviour
{
    private string cname; //"name"だとエラー
    private CharacterFeature feature;
    private CharacterInfo info;

    public Character(string name, CharacterFeature feature, CharacterInfo info) 
    {
        this.cname = name;
        this.feature = feature;
        this.info = info;
    }

    public abstract void Move();

    public abstract void Jump();

}
