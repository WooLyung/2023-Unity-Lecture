using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

public abstract class EmitEvent : Decodable
{
    public abstract byte[] ToBinary();
}