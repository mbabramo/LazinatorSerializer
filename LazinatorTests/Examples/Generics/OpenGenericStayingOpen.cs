using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Core;

namespace LazinatorTests.Examples.Generics
{
    public partial class OpenGenericStayingOpen<T> : IOpenGenericStayingOpen<T> where T : ILazinator, new()
    {
        public List<T> MyListT { get; set; }
    }
}
