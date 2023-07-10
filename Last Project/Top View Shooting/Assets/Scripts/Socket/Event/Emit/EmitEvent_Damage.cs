using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;
using UnityEngine;

public class EmitEvent_Damage : EmitEvent
{
    public int id;
    public int victim;
    public int damage;

    public EmitEvent_Damage(int id, int victim, int damage)
    {
        this.id = id;
        this.victim = victim;
        this.damage = damage;
    }

    public override byte[] ToBinary()
    {
        return ByteUtil.UnzipWrapped(2, new List<byte[]>() { // 이벤트 아이디 = 2
            ByteUtil.From(id),
            ByteUtil.From(victim),
            ByteUtil.From(damage)
        });
    }
}