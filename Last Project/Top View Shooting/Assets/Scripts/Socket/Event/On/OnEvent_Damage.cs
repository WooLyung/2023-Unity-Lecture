using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class OnEvent_Damage : OnEvent
{
    public int id;
    public int victim;
    public int damage;

    public OnEvent_Damage(byte[] buffer)
    {
        id = ByteUtil.ToInt(buffer, 0);
        victim = ByteUtil.ToInt(buffer, 4);
        damage = ByteUtil.ToInt(buffer, 8);
    }
}
