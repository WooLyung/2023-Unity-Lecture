using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EmitEvent_Update : EmitEvent
{
    public string evt = "update";
    public float x;
    public float y;
    public float angle;

    public EmitEvent_Update(float x, float y, float angle)
    {
        this.x = x;
        this.y = y;
        this.angle = angle;
    }
}