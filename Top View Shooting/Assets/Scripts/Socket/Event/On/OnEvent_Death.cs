using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class OnEvent_Death : OnEvent
{
    public int id;
    public int victim;

    public OnEvent_Death(byte[] buffer)
    {
        id = ByteUtil.ToInt(buffer, 0);
        victim = ByteUtil.ToInt(buffer, 4);
    }
}
