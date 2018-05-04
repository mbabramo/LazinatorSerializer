using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lazinator.Core;

namespace Lazinator.Collections.Avl
{
	public sealed partial class AvlNode<TKey, TValue> : IAvlNode<TKey, TValue> where TKey : ILazinator, new() where TValue : ILazinator, new()
	{
        // We can't serialize the Parent, so we use the Lazinator built-in parent as a substitute.

	    private AvlNode<TKey, TValue> _Parent;
	    public AvlNode<TKey, TValue> Parent
	    {
	        get
	        {
	            if (_Parent == null)
	            {
                    if (LazinatorParentClass is AvlTree<TKey, TValue> p)
	                    _Parent = p.Root;
                    else
                        _Parent = (AvlNode<TKey, TValue>)LazinatorParentClass;
                }

	            if (_Parent == this)
	                throw new Exception("DEBUG -- internal error");

	            return _Parent;
	        }
	        set { _Parent = value; }
	    }
    }
}
