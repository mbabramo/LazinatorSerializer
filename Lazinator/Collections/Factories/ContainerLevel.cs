using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public partial class ContainerLevel : IContainerLevel
    {
        public ContainerLevel(ContainerType valueContainerType)
        {
            ContainerType = valueContainerType;
            SplitThreshold = long.MaxValue;
            Unbalanced = false;
            AllowDuplicates = false;
        }

        public ContainerLevel(ContainerType valueContainerType, bool allowDuplicates)
        {
            ContainerType = valueContainerType;
            SplitThreshold = long.MaxValue;
            Unbalanced = false;
            AllowDuplicates = allowDuplicates;
        }

        public ContainerLevel(ContainerType valueContainerType, long splitThreshold, bool unbalanced, bool allowDuplicates)
        {
            ContainerType = valueContainerType;
            SplitThreshold = splitThreshold;
            Unbalanced = unbalanced;
            AllowDuplicates = allowDuplicates;
        }

        public ContainerType ContainerType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Unbalanced { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool AllowDuplicates { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public long SplitThreshold { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
