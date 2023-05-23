using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class EmitEvent_Update : EmitEvent
{
    public float x;
    public float y;
    public float angle;

    public EmitEvent_Update(float x, float y, float angle)
    {
        this.x = x;
        this.y = y;
        this.angle = angle;
    }

    public override int GetCode()
    {
        return 1;
    }

    public override List<byte[]> ToBinary()
    {
        return null;
    }
}