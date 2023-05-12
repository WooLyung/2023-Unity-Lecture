using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class OnEvent_Update : OnEvent
{
    [Serializable]
    public class Player
    {
        public string id;
        public float x;
        public float y;
        public float angle;
    }

    public int hp;
    public Player[] others;
}