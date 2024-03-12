using System;

namespace CountedTree.Node
{
    public partial class IncludedIndices : IIncludedIndices
    {

        public IncludedIndices(uint firstIndexInSuperset, uint lastIndexInSuperset, uint firstIndexInFilteredSet, uint lastIndexInFilteredSet)
        {
            FirstIndexInSuperset = firstIndexInSuperset;
            LastIndexInSuperset = lastIndexInSuperset;
            FirstIndexInFilteredSet = firstIndexInFilteredSet;
            LastIndexInFilteredSet = lastIndexInFilteredSet;
        }

        public override bool Equals(object obj)
        {
            IncludedIndices other = obj as IncludedIndices;
            if (other == null)
                return false;
            return FirstIndexInSuperset == other.FirstIndexInSuperset && LastIndexInSuperset == other.LastIndexInSuperset && FirstIndexInFilteredSet == other.FirstIndexInFilteredSet && LastIndexInFilteredSet == other.LastIndexInFilteredSet;
        }

        public override int GetHashCode()
        {
            return new Tuple<uint, uint, uint, uint>(FirstIndexInSuperset, LastIndexInSuperset, FirstIndexInFilteredSet, LastIndexInFilteredSet).GetHashCode();
        }

        public override string ToString()
        {
            if (FirstIndexInSuperset == FirstIndexInFilteredSet && LastIndexInSuperset == LastIndexInFilteredSet)
                return $"({FirstIndexInSuperset}, {LastIndexInSuperset})";
            else
                return $"S:({FirstIndexInSuperset}, {LastIndexInSuperset}) F:({FirstIndexInFilteredSet}, {LastIndexInFilteredSet})";
        }
    }
}
