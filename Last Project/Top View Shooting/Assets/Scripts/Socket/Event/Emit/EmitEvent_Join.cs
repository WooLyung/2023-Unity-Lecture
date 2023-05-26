using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

public class EmitEvent_Join : EmitEvent
{
    public string nickname;
    public string color;

    public EmitEvent_Join(string nickname, string color)
    {
        if (nickname.Length < 16)
        {
            for (int i = 16 - nickname.Length; i >= 1; i--)
                nickname += " ";
        }
        else if (nickname.Length > 16)
        {
            nickname = nickname.Substring(0, 16);
        }
        this.nickname = nickname;
        this.color = color;
    }

    public override byte[] ToBinary()
    {
        return ByteUtil.UnzipWrapped(0, new List<byte[]>() {
            ByteUtil.From(nickname),
            ByteUtil.From(color)
        });
    }
}