using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{

    private Rigidbody2D rig;
    private GetObjectsAround getObjectsAround;
    public Human human;

    public float walkspeed = 3.0f;

    public float jumpforce = 200f;

    public int direction = 0;

    public bool isJump = false;

    public MoveType moveType;

    public Vector2 targetPosition;

    public bool isOnLadder = false;

    void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        getObjectsAround = GetComponent<GetObjectsAround>();
        human = GetComponent<Human>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!BlockManager.Instance.ladderMap[human.worldPos.x, human.worldPos.y])
        {
            isOnLadder = false;
        }
        if(isOnLadder)
        {
            rig.bodyType = RigidbodyType2D.Kinematic;
        }
        else
        {
            rig.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    public IEnumerator Move(Vector2? targetPos1, float distance)
    {
        while(true)
        {
            if(targetPos1 != null)
            {
                Vector2 targetPos = (Vector2)targetPos1;
                List<MoveInfo> moveInfos = SearchRoot(targetPos, distance);
                if(moveInfos == null)
                {
                    yield return false;
                    yield break;
                }
                for(int i = 0; i < moveInfos.Count; i++)
                {
                    switch(moveInfos[i].moveType)
                    {
                        case MoveType.walk:
                        moveType = MoveType.walk;
                        targetPosition = moveInfos[i].targetPosition;
                        if(i + 1 < moveInfos.Count && (moveInfos[i + 1].moveType == MoveType.radder || moveInfos[i + 1].moveType == MoveType.fall))
                        {
                            yield return StartCoroutine(MoveToPosition(moveInfos[i].targetPosition, 0.2f));
                        }
                        else
                        {
                            yield return StartCoroutine(MoveToPosition(moveInfos[i].targetPosition, 1.0f));
                        }
                        break;

                        case MoveType.radder:
                        moveType = MoveType.radder;
                        targetPosition = moveInfos[i].targetPosition;
                        yield return StartCoroutine(MoveOnLadder(moveInfos[i].targetPosition));
                        break;

                        case MoveType.fall:
                        moveType = MoveType.fall;
                        targetPosition = moveInfos[i].targetPosition;
                        yield return StartCoroutine(Fall(moveInfos[i].targetPosition));
                        break;
                    }
                }
                moveType = MoveType.noType;
                yield return true;
                yield break;
            }
            else
            {
                yield return false;
                yield break;
            }
        }


    }

    //目的地に向かってキャラを単純移動させる、1段までの段差は自動的にジャンプする
    public IEnumerator MoveToPosition(Vector2 pos, float distance)
    {
        Debug.Log("Move start");
        bool[,] ladderMap = BlockManager.Instance.ladderMap;
        Coroutine coroutine = StartCoroutine(DecisionJump());
        while(Mathf.Abs(pos.x - rig.transform.position.x) > distance || Mathf.Abs(pos.y - rig.transform.position.y) > distance)
        {
            if(pos.x > rig.position.x)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }

            SetWalk(direction * walkspeed);

            if(ladderMap[human.worldPos.x, human.worldPos.y] && Mathf.Abs(rig.transform.position.x - pos.x) > Model.BLOCK_SIZE * 0.5f)
            {
                isOnLadder = true;
            }
            else
            {
                isOnLadder = false;
            }

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

    //はしご上で移動する
    public IEnumerator MoveOnLadder(Vector2? pos1)
    {
        bool[,] ladderMap = BlockManager.Instance.ladderMap;
        if(pos1 != null)
        {
            Vector2 pos = (Vector2)pos1;
            while(ladderMap[human.worldPos.x, human.worldPos.y])
            { 
                Vector2 velocity = rig.velocity;
                isOnLadder = true;
                if(transform.position.y < pos.y)
                {
                    velocity.y = 1.0f;
                }
                else
                {
                    velocity.y = -1.0f;
                }
                if(pos.x - transform.position.x > 0.2f)
                {
                    velocity.x = 0.05f;
                }
                else if(pos.x - transform.position.x < -0.2f)
                {
                    velocity.x = -0.05f;
                }
                else
                {
                    velocity.x = 0f;
                }
                rig.velocity = velocity;
                if(Mathf.Abs(pos.y - rig.transform.position.y) < Model.BLOCK_SIZE * 0.2f)
                {
                    velocity.y = 0;
                    rig.velocity = velocity;
                    yield break;
                }
                yield return null;
            }
            isOnLadder = false;
        }
        yield break;

    }

    //ただ落ちる
    public IEnumerator Fall(Vector2 pos)
    {
        StopWalk();
        while(Vector2.Distance(transform.position, pos) > 0.1f)
        {
            yield return null;
        }
        yield break;
    }

    public List<MoveInfo> SearchRoot(Vector2 pos, float distance)
    {
        List<MoveInfo> moveInfos = new List<MoveInfo>();
        Vector2Int curWorldPos = World.GetWorldPosition(gameObject.transform.position);
        Vector2Int targetWorldPos = World.GetWorldPosition(pos);
        Vector2Int? underBlockPos;
        List<BranchInfo> branchInfos = new List<BranchInfo>();
        bool[,] blockMap = (bool[,])BlockManager.Instance.blockMap.Clone();
        bool[,] ladderMap = (bool[,])BlockManager.Instance.ladderMap.Clone();
        bool[,] passedWay = new bool[(int)(Model.WORLD_WIDTH / Model.BLOCK_SIZE), (int)(Model.WORLD_HEIGHT / Model.BLOCK_SIZE)];
        bool[] objectsAround = new bool[10];
        int num = 0;
        for(int i = 0; i < (int)(Model.WORLD_HEIGHT / Model.BLOCK_SIZE); i++)
        {
            for(int h = 0; h < (int)(Model.WORLD_WIDTH / Model.BLOCK_SIZE); h++)
            {
                passedWay[h, i] = false;
            }
        }


        
        BranchInfo branchInfo = GetFirstBranch();
        branchInfos.Add(branchInfo);
        
        while(true)
        {
            if(branchInfos.Count <= 0)
            {
                Debug.Log("no root");
                break;
            }
            Debug.Log(1);
            
            if(branchInfos[branchInfos.Count - 1].ways.Count > 0)
            {
                curWorldPos = branchInfos[branchInfos.Count - 1].position;
                Debug.Log(2);


                //Wayの探索順の最適化
                List<Way> list = branchInfos[branchInfos.Count - 1].ways;
                List<Way> ways = new List<Way>();
                if(curWorldPos.x < targetWorldPos.x)
                {
                    if(list.Contains(Way.right))
                    {
                        ways.Add(Way.right);
                    }
                    if(curWorldPos.y < targetWorldPos.y)
                    {
                        if(list.Contains(Way.up))
                        {
                            ways.Add(Way.up);
                        }
                        if(list.Contains(Way.down))
                        {
                            ways.Add(Way.down);
                        }
                    }
                    else
                    {
                        if(list.Contains(Way.down))
                        {
                            ways.Add(Way.down);
                        }
                        if(list.Contains(Way.up))
                        {
                            ways.Add(Way.up);
                        }
                    }
                    if(list.Contains(Way.left))
                    {
                        ways.Add(Way.left);
                    }
                }
                else
                {
                    if(list.Contains(Way.left))
                    {
                        ways.Add(Way.left);
                        if(curWorldPos.y < targetWorldPos.y)
                        {
                            if(list.Contains(Way.up))
                            {
                                ways.Add(Way.up);
                            }
                            if(list.Contains(Way.down))
                            {
                                ways.Add(Way.down);
                            }
                        }
                        else
                        {
                            if(list.Contains(Way.down))
                            {
                                ways.Add(Way.down);
                            }
                            if(list.Contains(Way.up))
                            {
                                ways.Add(Way.up);
                            }
                        }
                    }
                    if(curWorldPos.y < targetWorldPos.y)
                    {
                        if(list.Contains(Way.up))
                        {
                            ways.Add(Way.up);
                        }
                        if(list.Contains(Way.down))
                        {
                            ways.Add(Way.down);
                        }
                    }
                    else
                    {
                        if(list.Contains(Way.down))
                        {
                            ways.Add(Way.down);
                        }
                        if(list.Contains(Way.up))
                        {
                            ways.Add(Way.up);
                        }
                    }
                    if(list.Contains(Way.right))
                    {
                        ways.Add(Way.right);
                    }
                }
                if(list.Contains(Way.fall))
                {
                    ways.Add(Way.fall);
                }
                BranchInfo a = branchInfos[branchInfos.Count - 1];
                a.ways = ways;
                branchInfos[branchInfos.Count - 1] = a;
                Debug.Log(branchInfos[branchInfos.Count - 1].ways[0].ToString());
                Debug.Log(3);
                
                switch(branchInfos[branchInfos.Count - 1].ways[0])
                {
                    case Way.left:
                    Debug.Log(4);
                    branchInfo = GoNextBranchOnGround(-1);
                    break;

                    case Way.right:
                    Debug.Log(4);
                    branchInfo = GoNextBranchOnGround(1);
                    break;

                    case Way.down:
                    Debug.Log(4);
                    branchInfo = GoNextBranchOnLadder(-1);
                    break;

                    case Way.up:
                    Debug.Log(4);
                    branchInfo = GoNextBranchOnLadder(1);
                    
                    break;

                    case Way.fall:
                    Debug.Log(4);
                    branchInfo = GoNextBranchOnFalling();
                    break;
                }
            
                if(!branchInfo.isGoal)
                {
                    if(branchInfo.ways.Count == 0)//行き止まりかどうか
                    {
                        Debug.Log(branchInfos[branchInfos.Count - 1].ways[0] + "Wayremove");
                        branchInfos[branchInfos.Count - 1].ways.RemoveAt(0);
                    }
                    else
                    {
                        Debug.Log("Add branch");
                        branchInfos.Add(branchInfo);
                    }
                }
                else
                {
                    branchInfos.Add(branchInfo);
                    Debug.Log("Goal!");
                    for(int i = 0; i < branchInfos.Count - 1; i++)
                    {
                        switch(branchInfos[i].ways[0])
                        {
                            case Way.up:
                            case Way.down:
                            moveInfos.Add(new MoveInfo(MoveType.radder, World.GetPositon(branchInfos[i + 1].position)));
                            break;

                            case Way.left:
                            case Way.right:
                            moveInfos.Add(new MoveInfo(MoveType.walk, World.GetPositon(branchInfos[i + 1].position)));
                            break;

                            case Way.fall:
                            moveInfos.Add(new MoveInfo(MoveType.fall, World.GetPositon(branchInfos[i + 1].position)));
                            break;

                        }
                    }
                    return moveInfos;
                }
                
            }
            else
            {
                if(branchInfos.Count > 0)
                {
                    Debug.Log(branchInfos[branchInfos.Count - 1].position + "branchRemove");
                    if(branchInfos.Count > 1)
                    {
                        branchInfos[branchInfos.Count - 2].ways.RemoveAt(0);
                    }
                    branchInfos.RemoveAt(branchInfos.Count - 1);
                    if(branchInfos.Count == 0)
                    {
                        Debug.Log("no root");
                        break;
                    }
                }
                else
                {
                    Debug.Log("no root");
                    break;
                }
            }

            if(num++ > 500)
            {
                break;
            }
            Debug.Log(5);
        }

        return null;

        BranchInfo GoNextBranchOnGround(int direction)
        {
            int num = 0;
            Debug.Log(6);
            while(true)
            {
                num++;
                Debug.Log("Go next" + num);
                ResetObjectsAround();
                GetObjectsAroundOnGround(direction);
                Debug.Log(12345);
                
                //到達判定、距離が近く、下にブロックが存在している時到達
                if(Mathf.Abs(curWorldPos.x - targetWorldPos.x) < distance && Mathf.Abs(curWorldPos.y - targetWorldPos.y) < distance && objectsAround[4])
                {
                    Debug.Log("Goal");
                    return new BranchInfo(curWorldPos, new List<Way>(), true);
                }
                List<Way> ways = new List<Way>();
                //はしご、落下の分岐判定 初回は判定しない
                if(num > 1)
                {
                    //すでに通過したか判定
                    if(passedWay[curWorldPos.x, curWorldPos.y])
                    {
                        return new BranchInfo(curWorldPos, ways, false);
                    }
                    if(objectsAround[6])
                    {
                        
                        if(ladderMap[curWorldPos.x, curWorldPos.y + 1])
                        {
                            ways.Add(Way.up);
                        }
                        if(ladderMap[curWorldPos.x, curWorldPos.y - 1])
                        {
                            ways.Add(Way.down);
                        }
                        else if(!objectsAround[4])
                        {
                            ways.Add(Way.fall);
                        }
                        if(!objectsAround[3])
                        {
                            if(direction == 1)
                            {
                                ways.Add(Way.right);
                            }
                            else
                            {
                                ways.Add(Way.left);
                            }
                        }
                        return new BranchInfo(curWorldPos, ways, false);
                    }
                    if(!objectsAround[4])
                    {
                        return new BranchInfo(curWorldPos, new List<Way>(){Way.fall}, false);
                    }
                    passedWay[curWorldPos.x, curWorldPos.y] = true;
                }

                //次の移動場所、行き止まりの判定
                int movePos = MoveAblePositionOnGround();
                if(movePos == 5 || movePos == 3 || movePos == 2)
                {
                    MoveOnGround(movePos, direction);
                    Debug.Log("movePos" + movePos);
                }
                else
                {
                    Debug.Log("movePos" + movePos);
                    return new BranchInfo(curWorldPos, new List<Way>(), false);
                }
                if(num > 100)
                {
                    return new BranchInfo(curWorldPos, new List<Way>(), false);
                }
            }

        }

        BranchInfo GoNextBranchOnLadder(int upOrDown)
        {
            int num = 0;
            while(true)
            {
                num++;
                ResetObjectsAround();
                bool isBranch = false;
                GetObjectsAroundOnLadder(upOrDown);
                Debug.Log(4);
                Debug.Log(objectsAround[6]);
                Debug.Log(BlockManager.Instance.ladderMap[curWorldPos.x, curWorldPos.y + 1 * upOrDown]);

                //到達判定
                if(Mathf.Abs(curWorldPos.x - targetWorldPos.x) < distance && Mathf.Abs(curWorldPos.y - targetWorldPos.y) < distance)// && objectsAround[6]
                {
                    return new BranchInfo(curWorldPos, new List<Way>(), true);
                }

                List<Way> ways = new List<Way>();

                //2回目以降
                if(num > 1)
                {
                    //すでに通過したか判定
                    if(passedWay[curWorldPos.x, curWorldPos.y])
                    {
                        return new BranchInfo(curWorldPos, ways, false);
                    }
                    //横への分岐判定
                    if(objectsAround[4] && !objectsAround[2])
                    {
                        ways.Add(Way.left);
                        isBranch = true;
                    }
                    if(objectsAround[5] && !objectsAround[3])
                    {
                        ways.Add(Way.right);
                        isBranch = true;
                    }

                    if(upOrDown == -1 && !objectsAround[1] && !objectsAround[6])
                    {
                        ways.Add(Way.fall);
                        return new BranchInfo(curWorldPos, ways, false);
                    }
                    passedWay[curWorldPos.x, curWorldPos.y] = true;
                }
                
                int movePos = MoveAblePositonOnLadder(upOrDown);
                //行き止まりの判定
                if(movePos == 1)
                {
                    if(!isBranch)
                    {
                        MoveOnLadder(1, upOrDown);
                    }
                    else
                    {
                        if(upOrDown == 1)
                        {
                            ways.Add(Way.up);
                            return new BranchInfo(curWorldPos, ways, false);
                        }
                        else
                        {
                            ways.Add(Way.down);
                            return new BranchInfo(curWorldPos, ways, false);
                        }
                    }
                }
                else
                {
                    return new BranchInfo(curWorldPos, ways, false);
                }
            }
            
        }

        BranchInfo GoNextBranchOnFalling()
        {
            Debug.Log(4);
            underBlockPos = null;
            GetObjectsUnderOnFalling();
            Debug.Log(5);
            if(underBlockPos != null)
            {
                curWorldPos = (Vector2Int)underBlockPos + Vector2Int.up;
                Debug.Log(curWorldPos);
                return new BranchInfo(curWorldPos, new List<Way>(){Way.left, Way.right}, false);
            }
            else
            {
                return new BranchInfo(curWorldPos, new List<Way>(), false);
            }
        }

        BranchInfo GetFirstBranch()
        {
            ResetObjectsAround();
            objectsAround[1] = (curWorldPos.x - 1 >= 0 && curWorldPos.y + 1 < Model.WORLD_HEIGHT) ? blockMap[curWorldPos.x - 1, curWorldPos.y + 1] : false;
            objectsAround[2] = (curWorldPos.y + 1 < Model.WORLD_HEIGHT) ? blockMap[curWorldPos.x, curWorldPos.y + 1] : false;
            objectsAround[3] = (curWorldPos.x + 1 < Model.WORLD_WIDTH && curWorldPos.y + 1 < Model.WORLD_HEIGHT) ? blockMap[curWorldPos.x + 1, curWorldPos.y + 1] : false;
            objectsAround[4] = (curWorldPos.x - 1 >= 0) ? blockMap[curWorldPos.x - 1, curWorldPos.y] : false;
            objectsAround[5] = (curWorldPos.x + 1 < Model.WORLD_WIDTH) ? blockMap[curWorldPos.x + 1, curWorldPos.y] : false;
            objectsAround[6] = (curWorldPos.x - 1 >= 0 && curWorldPos.y - 1 >= 0) ? blockMap[curWorldPos.x - 1, curWorldPos.y - 1] : false;
            objectsAround[7] = (curWorldPos.y - 1 >= 0) ? blockMap[curWorldPos.x, curWorldPos.y - 1] : false;
            objectsAround[8] = (curWorldPos.x + 1 < Model.WORLD_WIDTH && curWorldPos.y - 1 >= 0) ? blockMap[curWorldPos.x + 1, curWorldPos.y - 1] : false;
            objectsAround[9] = (curWorldPos.y + 1 < Model.WORLD_HEIGHT) ? ladderMap[curWorldPos.x, curWorldPos.y + 1] : false;
            objectsAround[0] = (curWorldPos.y - 1 >= 0) ? ladderMap[curWorldPos.x, curWorldPos.y - 1] : false;
            
            List<Way> ways = new List<Way>();
            if(objectsAround[0] && ladderMap[curWorldPos.x, curWorldPos.y])
            {
                ways.Add(Way.down);
            }
            if(objectsAround[9] && ladderMap[curWorldPos.x, curWorldPos.y])
            {
                ways.Add(Way.up);
            }
            if(!objectsAround[7])
            {
                ways.Add(Way.fall);
                if(!objectsAround[4] && objectsAround[6])
                {
                    ways.Add(Way.left);
                }
                if(!objectsAround[5] && objectsAround[8])
                {
                    ways.Add(Way.right);
                }
            }
            else
            {
                if(!objectsAround[4] || (objectsAround[4] && !objectsAround[1] && !objectsAround[2]))
                {
                    ways.Add(Way.left);
                }
                if(!objectsAround[5] || (objectsAround[5] && !objectsAround[3] && !objectsAround[2]))
                {
                    ways.Add(Way.right);
                }
            }
            return new BranchInfo(curWorldPos, ways, false);
            
        }

        void GetObjectsAroundOnGround(int direction)
        {
            //direction = 1の場合のブロックの有無の配列
            //(2)1 2
            //(3)人3
            //(5)4 5 6は人の位置のはしご判定
            objectsAround[1] = (curWorldPos.y + 1 < Model.WORLD_HEIGHT) ? blockMap[curWorldPos.x, curWorldPos.y + 1] : false;
            objectsAround[2] = (curWorldPos.x + 1 < Model.WORLD_WIDTH && curWorldPos.x - 1 >= 0 && curWorldPos.y + 1 < Model.WORLD_HEIGHT) ? blockMap[curWorldPos.x + direction, curWorldPos.y + 1] : false;
            objectsAround[3] = (curWorldPos.x + 1 < Model.WORLD_WIDTH && curWorldPos.x - 1 >= 0) ? blockMap[curWorldPos.x + direction, curWorldPos.y] : false;
            objectsAround[4] = (curWorldPos.y - 1 >= 0) ? blockMap[curWorldPos.x, curWorldPos.y - 1]: false;
            objectsAround[5] = (curWorldPos.x + 1 < Model.WORLD_WIDTH && curWorldPos.x - 1 >= 0 && curWorldPos.y - 1 >= 0) ? blockMap[curWorldPos.x + direction, curWorldPos.y - 1] : false;
            objectsAround[6] = ladderMap[curWorldPos.x, curWorldPos.y];
        
        }

        void GetObjectsAroundOnLadder(int upOrDown)
        {
            //direction = 1の場合のブロックの有無の配列
            //  1 
            //2 人 3
            //4 (1)5  6は1の場所にはしごがあるかどうか
            objectsAround[1] = (curWorldPos.y + upOrDown >= 0 && curWorldPos.y + upOrDown < Model.WORLD_HEIGHT) ? blockMap[curWorldPos.x, curWorldPos.y + 1 * upOrDown]: false;
            objectsAround[2] = (curWorldPos.x - 1 >= 0) ? blockMap[curWorldPos.x - 1, curWorldPos.y] : false;
            objectsAround[3] = (curWorldPos.x + 1 < Model.WORLD_WIDTH) ? blockMap[curWorldPos.x + 1, curWorldPos.y] : false;
            objectsAround[4] = (curWorldPos.x - 1 >= 0 && curWorldPos.y - 1 >= 0) ? blockMap[curWorldPos.x - 1, curWorldPos.y - 1] : false;
            objectsAround[5] = (curWorldPos.x + 1 < Model.WORLD_WIDTH && curWorldPos.y - 1 >= 0) ? blockMap[curWorldPos.x + 1, curWorldPos.y - 1] : false;
            objectsAround[6] = (curWorldPos.y + upOrDown >= 0 && curWorldPos.y + upOrDown < Model.WORLD_HEIGHT) ? ladderMap[curWorldPos.x, curWorldPos.y + 1 * upOrDown] : false;
            
        }

        void GetObjectsUnderOnFalling()
        {
            for(int i = -1; i > -10; i--)
            {
                if(curWorldPos.y + i >= 0)
                {
                    if(blockMap[curWorldPos.x, curWorldPos.y + i])
                    {
                        underBlockPos = new Vector2Int(curWorldPos.x, curWorldPos.y + i);
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        int MoveAblePositionOnGround()
        {  
            if(!objectsAround[3] && !objectsAround[5])
            {
                return 5;
            }
            else if(!objectsAround[3])
            {
                return 3;
            }
            else if(!objectsAround[1] && !objectsAround[2])
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }

        int MoveAblePositonOnLadder(int upOrDown)
        {
            if(objectsAround[6])
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        void MoveOnGround(int pos, int direction)
        {
            switch(pos)
            {
                case 2:
                curWorldPos += Vector2Int.up + Vector2Int.right * direction;
                break;

                case 3:
                curWorldPos += Vector2Int.right * direction;
                break;

                case 5:
                curWorldPos += Vector2Int.right * direction + Vector2Int.down;
                break;

            }
        }

        void MoveOnLadder(int pos, int upOrDown)
        {
            switch(pos)
            {
                case 1:
                curWorldPos += Vector2Int.up * upOrDown;
                break;
            }
        }
    
        void ResetObjectsAround()
        {
            for(int i = 0; i < 10; i++)
            {
                objectsAround[i] = false;
            }
        }


        



}

    public struct MoveInfo
    {
        public MoveType moveType;
        public Vector2 targetPosition;

        public MoveInfo(MoveType moveType, Vector2 targetPosition)
        {
            this.moveType = moveType;
            this.targetPosition = targetPosition;
        }
    }

    public enum MoveType
    {
        walk,
        radder,
        fall,
        noType,
    }

    public struct BranchInfo
    {
        public Vector2Int position;
        public List<Way> ways;
        public bool isGoal;

        public BranchInfo(Vector2Int position, List<Way> ways, bool isGoal)
        {
            this.position = position;
            this.ways = new List<Way>(ways);
            this.isGoal = isGoal;
        }
    }

    public enum Way
    {
        up,
        down,
        right,
        left,
        fall,
    }
}

