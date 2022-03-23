using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{    
    public Vector2Int worldPos;
    public CharacterMove charmove;

    public Instraction myInstraction;
    public string inst;
    public Material standardMaterial;

    public Material outLineMaterial;
    private bool isInstractionChanged = false;
    private Coroutine currentCouroutine = null;

    private SpriteRenderer spriteRenderer;

    public bool isSelected = false;

    // Start is called before the first frame update

    void Start()
    {
        transform.parent = GameObject.Find ("HumanManager").transform;
        charmove = GetComponent<CharacterMove>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        InitInstraction();
        StartCoroutine(ExecuteInstraction());
    }

    // Update is called once per frame
    void Update()
    {
        inst = myInstraction.type.ToString();
        if(isSelected)
        {
            spriteRenderer.material = outLineMaterial;
        }
        else
        {
            spriteRenderer.material = standardMaterial;
        }
        
        worldPos = World.GetWorldPosition(transform.position);
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

    private void InitInstraction()
    {
        ReceiveInstraction(InstractionManager.Instance.CleateInstraction(InstractionType.noInstraction, null));    
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
                    currentCouroutine = StartCoroutine(MoveToPosition(myInstraction));
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

                    case InstractionType.noInstraction:
                    break;

                    default:
                    break;
                }
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    public bool GetIsMoveAble(Vector2 pos)
    {
        if(charmove.SearchRoot(pos, 2.0f) != null)
        {
            return true;
        }
        else
        {
            return false;
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
        if(instraction != null)
        {
            instraction.state = InstractionState.finished;
        }
        StopActionCoroutines();
    }

    public void CancelInstraction(Instraction instraction)
    {
        if(instraction != null)
        {
            instraction.state = InstractionState.cancel;
        }
        StopActionCoroutines();
    }

    public IEnumerator MoveToPosition(Instraction instraction)
    {
        IEnumerator coroutine = charmove.Move(instraction.target?.transform.position, 0.5f);
        yield return StartCoroutine(coroutine);
        if(!(bool)coroutine.Current)
        {
            CancelInstraction(instraction);
        }
        FinishInstraction(instraction);
        SetMovePoint.Instance.DeleteMoveTarget(instraction.target);
        yield break;
    }

    public IEnumerator Attack(Instraction instraction)
    {
        FinishInstraction(instraction);
        yield break;

    }

    public IEnumerator SetBlock(Instraction instraction)
    {
        IEnumerator coroutine = charmove.Move(instraction.target?.transform.position, 2.0f);
        yield return StartCoroutine(coroutine);
        if(!(bool)coroutine.Current)
        {
            CancelInstraction(instraction);
        }
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
        IEnumerator coroutine = charmove.Move(instraction.target?.transform.position, 2.0f);
        yield return StartCoroutine(coroutine);
        if(!(bool)coroutine.Current)
        {
            CancelInstraction(instraction);
        }
        do
        {
            yield return new WaitForSeconds(1.0f);
            block.hp -= 5.0f;
        } 
        while(block.hp > 0f);
        FinishInstraction(instraction);
        yield break;
    }

    private void OnCollisionStay2D(Collision2D other) {
        if(other.gameObject.CompareTag("Block"))
        {
            if(PhysicsFunc.isThereAnyObjectOnThePoint(transform.position, Model.BLOCK_LAYER))
            {
                Vector2 pos = transform.position;
                pos.y += Model.BLOCK_SIZE;
                transform.position = pos;
            }
        }
    }

}
