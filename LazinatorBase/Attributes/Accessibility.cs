using Lazinator.Support;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Attributes
{
    public enum Accessibility
    {
        [StringValue("public ")]
        Public,
        [StringValue("private ")]
        Private,
        [StringValue("protected ")]
        Protected,
        [StringValue("internal ")]
        Internal,
        [StringValue("protected internal ")]
        ProtectedInternal
    }
}
