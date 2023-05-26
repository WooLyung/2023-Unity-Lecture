using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class OnEvent_Leave : OnEvent
{
    public int id;

    public OnEvent_Leave(byte[] buffer)
    {
        id = ByteUtil.ToInt(buffer, 0);
    }
}
