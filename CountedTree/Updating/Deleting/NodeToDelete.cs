namespace CountedTree.Updating
{
    public partial class NodeToDelete : INodeToDelete
    {
        public NodeToDelete(long nodeID, bool isRoot)
        {
            NodeID = nodeID;
            IsRoot = isRoot;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 101;
                hash = hash * 107 + NodeID.GetHashCode();
                hash = hash * 111 + IsRoot.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            NodeToDelete other = obj as NodeToDelete;
            if (other == null)
                return false;
            return NodeID == other.NodeID || IsRoot == other.IsRoot;
        }

        public override string ToString()
        {
            return $"{NodeID}{(IsRoot ? "Root" : "")}";
        }
    }
}
