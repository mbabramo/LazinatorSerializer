using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Core
{
    /// <summary>
    /// An enum that will be a parameter in the constructor that Lazinator calls automatically. A separate constructor is used so that if there is a parameterless constructor called by client code, it will not be called every time the object is deserialized. 
    /// </summary>
    public enum LazinatorConstructorEnum : byte
    {
        LazinatorConstructor
    }
}
