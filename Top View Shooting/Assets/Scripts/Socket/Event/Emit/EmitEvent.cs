using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

public abstract class EmitEvent
{
    public abstract List<byte[]> ToBinary();

    public abstract int GetCode();
}