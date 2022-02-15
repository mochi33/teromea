using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{

    private Rigidbody2D rig;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetWalk(float walkspeed)
    {
        rig.velocity = new Vector2(walkspeed, 0);
    }

    public void StopWalk()
    {
        rig.velocity = Vector2.zero;
    }

    public void Jump(float jumpforce)
    {
        rig.AddForce(new Vector2(0, jumpforce));
    }

}
