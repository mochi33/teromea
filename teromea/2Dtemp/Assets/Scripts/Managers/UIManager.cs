using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Manipulation
{
    setBlock,
    dig,
}

public class UIManager : SingletonMonoBehaviour<UIManager>
{

    public GameObject selectDirtButtom;
    public GameObject selectStoneButtom;
    public Manipulation selectedManipulation = Manipulation.setBlock;
    public BlockType selectedBlockType = BlockType.dirt;
    // Start is called before the first frame update
    void Start()
    {
        selectDirtButtom = GameObject.Find("SelectDirt");
        selectStoneButtom = GameObject.Find("SelectStone");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectSetBlock()
    {
        selectedManipulation = Manipulation.setBlock;
        if(selectDirtButtom.activeSelf && selectStoneButtom.activeSelf)
        {
            selectDirtButtom.SetActive(false);
            selectStoneButtom.SetActive(false);
        }
        else
        {
            selectDirtButtom.SetActive(true);
            selectStoneButtom.SetActive(true);
        }
    }

    public void SelectDirtBlock()
    {
        selectedBlockType = BlockType.dirt;
    }

    public void SelectStoneBlock()
    {
        selectedBlockType = BlockType.stone;
    }

    public void SelectDig()
    {
        selectedManipulation = Manipulation.dig;
    }
}
