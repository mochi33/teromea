using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model
{
    public const float BLOCK_SIZE = 1.0f;

    public const int MAX_TEMPBLOCKS = 1000;

    public const int MAX_HUMAN = 1000;

    public const int MAX_INSTRACTION = 1000;

    public const float MAX_INSTRACTION_RANGE = 1000f;

    public const int MAX_ONINSTRACTION = 1000;

    public const System.Int32 BLOCK_LAYER = 1 << 6;
    public const System.Int32 TEMPBLOCK_LAYER = 1 << 7;

    public const System.Int32 HUMAN_LAYER = 1 << 8;

    public const System.Int32 MOVETARGET_LAYER = 1 << 9;
    public const System.Int32 LADDER_LAYER = 1 << 10;

    public const float WORLD_WIDTH = 100f;

    public const float WORLD_HEIGHT = 30f;
    public struct HUMAN_SIZE
    {
        public const float HEIGHT = 0.9f;
        public const float WIDTH = 0.5f;
    }

}
