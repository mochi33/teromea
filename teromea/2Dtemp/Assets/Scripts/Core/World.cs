using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class World
{
    
    public static bool isOnWorld(Vector2 pos)
    {
        return Mathf.Abs(pos.x) <= Model.WORLD_WIDTH && pos.y <= Model.WORLD_HEIGHT && pos.y > 0;
    }

    public static Vector2Int GetWorldPosition(Vector2 pos)
    {
        Vector2Int worldPos = new Vector2Int();
        worldPos.x = (int)Mathf.Floor(pos.x / Model.BLOCK_SIZE) + (int)(Model.WORLD_WIDTH / Model.BLOCK_SIZE / 2);
        worldPos.y = (int)Mathf.Floor(pos.y / Model.BLOCK_SIZE);

        return worldPos;
    }
    
    public static Vector2 GetPositon(Vector2Int worldPos)
    {
        return (new Vector2(worldPos.x - (Model.WORLD_WIDTH / 2) + 0.5f, worldPos.y + 0.5f) * Model.BLOCK_SIZE);
    }
}

