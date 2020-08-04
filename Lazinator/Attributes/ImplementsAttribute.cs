using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Used to indicate that a partial class or struct implements a particular method, so the code-behind should
    /// call the method implemented at the appropriate point and should not itself define the method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    public class ImplementsAttribute : Attribute
    {
        public string[] Implemented { get; set; }

        public ImplementsAttribute(string[] implemented)
        {
            Implemented = implemented;
        }

        // Note that it might seem that an alternative would be to use partial methods. That is, the code behind could
        // create a partial method that the user code could override with a full method. The problem is that we want the 
        // user code to override the code behind method. So, the code behind needs to know whether to implement its own method.
    }
}
