using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.BigList
{
    public class BigListUnsupportedOperationWithAppendOnlyException : Exception
    {
        public BigListUnsupportedOperationWithAppendOnlyException() : base("In an append-only big list, one can only insert at the end of the list, and one cannot remove items.")
        {
        }
    }
}
