using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Used to indicate that a partial class or struct implements a particular method, so the code-behind should
    /// call the method implemented at the appropriate point and should not itself call the method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    public class ImplementsAttribute : Attribute
    {
        public string[] Implemented { get; set; }

        public ImplementsAttribute(string[] implemented)
        {
            Implemented = implemented;
        }
    }
}
