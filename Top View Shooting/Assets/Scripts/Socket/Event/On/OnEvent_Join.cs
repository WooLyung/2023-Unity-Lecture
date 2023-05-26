using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class OnEvent_Join : OnEvent
{
    public int id;
    public string nickname;
    public string color;
    public float x;
    public float y;
    public float angle;
    public int hp;

    public OnEvent_Join(byte[] buffer)
    {
        id = ByteUtil.ToInt(buffer, 0);
        nickname = ByteUtil.ToString(buffer, 4, 16);
        color = ByteUtil.ToString(buffer, 20, 6);
        x = ByteUtil.ToFloat(buffer, 26);
        y = ByteUtil.ToFloat(buffer, 30);
        angle = ByteUtil.ToFloat(buffer, 34);
        hp = ByteUtil.ToInt(buffer, 38);
    }
}
