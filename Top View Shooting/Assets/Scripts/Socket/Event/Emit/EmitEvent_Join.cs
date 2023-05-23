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

    public override int GetCode()
    {
        return 0;
    }

    public override List<byte[]> ToBinary()
    {
        return new List<byte[]> {
            System.Text.Encoding.UTF8.GetBytes(nickname.ToCharArray()),
            System.Text.Encoding.UTF8.GetBytes(color.ToCharArray())
        };
    }
}