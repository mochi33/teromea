using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{

    private Rigidbody2D rig;
    public Vector2 targetposition;

    public float walkspeed = 3.0f;

    public float jumpforce = 200f;

    public float direction = 0;

    public bool isOnPosition = false;

    public bool isJump = false;

    public GameObject sideobject = null;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        Jump(jumpforce);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(targetposition, rig.position) < 0.1f)
        {
            isOnPosition = true;
        }

        if (Vector2.Distance(targetposition, rig.position) > 0.1f)
        {
            Vector2 position = rig.position;
            direction = (targetposition.x - position.x) / Mathf.Abs(targetposition.x - position.x);
            SetWalk(direction * walkspeed);
            if (sideobject != null && !isJump)
            {
                JumpToUpperPlace();
            }
        } else {
            StopWalk();
        }
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

    async void JumpToUpperPlace()
    {
        isJump = true;
        StopWalk();
        await Task.Delay(1000);
        Jump(jumpforce);
        sideobject = null;
        await Task.Delay(500);
        isJump = false;

    }

    private void OnCollisionEnter2D(Collision2D other) {
        GameObject obj = other.gameObject;
        if (obj.tag == "Block" && obj.transform.position.y + Model.BLOCK_SIZE > rig.position.y && !isOnPosition)
        {
            Debug.Log("collision!");
            sideobject = obj;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject == sideobject)
        {
            sideobject = null;
        }
    }

}
