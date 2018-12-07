using Lazinator.Collections.Dictionary;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lazinator.Collections
{
    /// <summary>
    /// A LazinatorGeneralTree with the ability to look up an element of the tree by location.
    /// </summary>
    /// <typeparam name="T">The type of each tree node</typeparam>
    public partial class LazinatorLocationAwareTree<T> : LazinatorGeneralTree<T>, ILazinatorLocationAwareTree<T> where T : ILazinator
    {
        public LazinatorLocationAwareTree(T item) : base(item)
        {
            OnAddChild(this);
        }

        public override LazinatorGeneralTree<T> CreateTree(T item)
        {
            return new LazinatorLocationAwareTree<T>(item);
        }

        private void InitializeLocationsIfNecessary()
        {
            if (Locations == null)
                Locations = new LazinatorDictionary<T, LazinatorList<WInt>>();
        }

        public LazinatorLocationAwareTree<T> GetLocationAwareRoot()
        {
            return (LazinatorLocationAwareTree<T>) GetRoot();
        }

        protected override void OnAddChild(LazinatorGeneralTree<T> child)
        {
            GetLocationAwareRoot().OnAddChildComplete(child);
        }

        private void OnAddChildComplete(LazinatorGeneralTree<T> child)
        {
            InitializeLocationsIfNecessary();
            var location = ConvertToLazinatorList(child.GetLocationInTree());
            Locations[child.Item] = location;
        }

        protected override void OnRemoveChild(LazinatorGeneralTree<T> child)
        {
            GetLocationAwareRoot().OnRemoveChildComplete(child);
        }

        private void OnRemoveChildComplete(LazinatorGeneralTree<T> child)
        {
            Locations.Remove(child.Item);
        }

        public LazinatorGeneralTree<T> GetTreeForItem(T item)
        {
            if (Locations.ContainsKey(item))
            {
                var locationAsLazinatorList = Locations[item];
                var locationAsList = ConvertToRegularList(locationAsLazinatorList);
                return GetTreeAtLocation(locationAsList);
            }
            return null;
        }

        private LazinatorList<WInt> ConvertToLazinatorList(List<int> list)
        {
            return new LazinatorList<WInt>(list.Select(x => new WInt(x)));
        }

        private List<int> ConvertToRegularList(LazinatorList<WInt> list)
        {
            return list.Select(x => x.WrappedValue).ToList();
        }
    }
}
