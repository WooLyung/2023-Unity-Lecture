using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;
using UnityEngine;

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

    public override byte[] ToBinary()
    {
        return ByteUtil.UnzipWrapped(1, new List<byte[]>() {
            ByteUtil.From(x),
            ByteUtil.From(y),
            ByteUtil.From(angle)
        });
    }
}