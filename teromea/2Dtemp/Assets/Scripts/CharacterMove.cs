using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{

    private Rigidbody2D rig;
    private GetObjectsAround getObjectsAround;
    public Vector2 targetposition;

    public float walkspeed = 3.0f;

    public float jumpforce = 200f;

    public int direction = 0;

    public bool isJump = false;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        getObjectsAround = GetComponent<GetObjectsAround>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator MoveToTarget(GameObject target)
    {
        Debug.Log("Move start");
        Coroutine coroutine = StartCoroutine(DecisionJump());
        while(!getObjectsAround.objectsAround.Contains(target))
        {
            if(target.transform.position.x > rig.position.x)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }

            SetWalk(direction * walkspeed);

            yield return null;
        }
        direction = 0;
        StopCoroutine(coroutine);
        StopWalk();
        yield break;

    }

    public IEnumerator MoveToPosition(Vector2 pos)
    {
        Debug.Log("Move start");
        Coroutine coroutine = StartCoroutine(DecisionJump());
        while(Vector2.Distance(targetposition, rig.position) > Model.BLOCK_SIZE * 0.2f)
        {
            if(targetposition.x > rig.position.x)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }

            SetWalk(direction * walkspeed);

            yield return null;
        }

        direction = 0;
        StopCoroutine(coroutine);
        StopWalk();
        yield break;
    }

    public void SetWalk(float walkspeed)
    {
        float yspeed = rig.velocity.y;
        rig.velocity = new Vector2(walkspeed, yspeed);
    }

    public void StopWalk()
    {
        float yspeed = rig.velocity.y;
        rig.velocity = new Vector2(0, yspeed);
    }

    public void Jump(float jumpforce)
    {
        Debug.Log("Jump!");
        
        rig.AddForce(new Vector2(0, jumpforce));
    }

    public IEnumerator JumpToUpperPlace()
    {
        isJump = true;
        StopWalk();
        yield return new WaitForSeconds(1.0f);
        Jump(jumpforce);
        yield return new WaitForSeconds(0.5f);
        isJump = false;
    }

    public IEnumerator DecisionJump()
    {
        isJump = false;
        LayerMask layerMask = 1 << 6;
        while(true)
        {
            if(!isJump
            && Physics2D.OverlapPointAll(new Vector2(rig.position.x + direction * Model.BLOCK_SIZE, rig.position.y), layerMask: layerMask).Length > 0
            && Mathf.Approximately(rig.velocity.y, 0))
            {
                yield return JumpToUpperPlace();
            }
            yield return null;
        }
    }

    // private void OnCollisionEnter2D(Collision2D other) {
    //     GameObject obj = other.gameObject;
    //     //obj.transform.position.y + Model.BLOCK_SIZE > rig.position.y
    //     Debug.Log(obj.transform.position.x - rig.position.x);
    //     Debug.Log(direction * Model.BLOCK_SIZE);
    //     if (obj.tag == "Block" 
    //     && (int)Mathf.Round(obj.transform.position.x - rig.position.x) == (int)Mathf.Round(direction * Model.BLOCK_SIZE) 
    //     && (int)Mathf.Round(obj.transform.position.y - rig.position.y) == 0
    //     && isMoving)
    //     {
    //         isJump = true;
    //     }
    // }

}
