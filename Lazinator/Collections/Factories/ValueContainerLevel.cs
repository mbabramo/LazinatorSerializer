using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public partial class ValueContainerLevel : IValueContainerLevel
    {
        public ValueContainerLevel(ValueContainerType valueContainerType, long splitThreshold, bool unbalanced, bool allowDuplicates)
        {
            ValueContainerType = valueContainerType;
            SplitThreshold = splitThreshold;
            Unbalanced = unbalanced;
            AllowDuplicates = allowDuplicates;
        }

        public ValueContainerType ValueContainerType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Unbalanced { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool AllowDuplicates { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public long SplitThreshold { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
