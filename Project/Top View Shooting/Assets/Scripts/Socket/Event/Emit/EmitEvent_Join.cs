using System;

[Serializable]
public class EmitEvent_Join : EmitEvent
{
    public string evt = "join";
    public string nickname;
    public string color;

    public EmitEvent_Join(string nickname, string color)
    {
        this.nickname = nickname;
        this.color = color;
    }
}