namespace CountedTree.Updating
{
    public partial class NodeToDeleteLater : INodeToDeleteLater
    {
        public NodeToDeleteLater(NodeToDelete nodeToDelete, long deleteWhenThisDeleted)
        {
            NodeToDelete = nodeToDelete;
            DeleteWhenThisDeleted = deleteWhenThisDeleted;
        }

        public override string ToString()
        {
            return $"Delete {NodeToDelete} when {DeleteWhenThisDeleted}";
        }
    }
}
