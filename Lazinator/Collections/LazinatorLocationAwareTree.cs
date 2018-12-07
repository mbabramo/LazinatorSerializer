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
    /// A LazinatorTree with the ability to look up an element of the tree by location.
    /// </summary>
    /// <typeparam name="T">The type of each tree node</typeparam>
    public partial class LazinatorLocationAwareTree<T> : LazinatorTree<T>, ILazinatorLocationAwareTree<T> where T : ILazinator
    {
        private void InitializeLocationsIfNecessary()
        {
            if (Locations == null)
                Locations = new LazinatorDictionary<T, LazinatorList<WInt>>();
        }

        public override void OnAddChild(LazinatorTree<T> child)
        {
            InitializeLocationsIfNecessary();
            var location = ConvertToLazinatorList(child.GetLocationInTree());
            Locations[child.Item] = location;
        }

        public override void OnRemoveChild(LazinatorTree<T> child)
        {
            Locations.Remove(child.Item);
        }

        public LazinatorTree<T> GetTreeForItem(T item)
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
