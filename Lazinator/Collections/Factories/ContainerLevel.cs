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
    }
}
