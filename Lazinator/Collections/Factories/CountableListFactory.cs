using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public class CountableListFactory<TKey> : ICountableListFactory<TKey> where TKey : ILazinator
    {
        public CountableListTypes ListType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ILazinatorCountableListable<TKey> Create()
        {
            switch (ListType)
            {
                case CountableListTypes.AvlList:
                    break;
                case CountableListTypes.AvlSortedList:
                    break;
            }
        }

        public ILazinatorCountableListable<U> Create2<U>() where U : ILazinator, IComparable<U>
        {
            return new SortedLazinatorList<U>();
        }
    }
}
