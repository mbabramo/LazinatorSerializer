using System;
using System.Collections;
using System.Collections.Generic;

namespace LazinatorCollections.Enumerators
{
    /// <summary>
    /// An enumerator that enumerates items from a source enumerator, but applies a transform function to each item before enumeration.
    /// </summary>
    /// <typeparam name="TSource">The type of the item in the source enumerator</typeparam>
    /// <typeparam name="TTarget">The type of the item once transformed</typeparam>
    public struct TransformEnumerator<TSource, TTarget> : IEnumerator<TTarget>
    {
        IEnumerator<TSource> SourceEnumerator;
        Func<TSource, TTarget> TransformFunc;

        public TransformEnumerator(IEnumerator<TSource> sourceEnumerator, Func<TSource, TTarget> transformFunc)
        {
            SourceEnumerator = sourceEnumerator;
            TransformFunc = transformFunc;
        }

        public TTarget Current => TransformFunc(SourceEnumerator.Current);

        object IEnumerator.Current => TransformFunc(SourceEnumerator.Current);

        public void Dispose()
        {
            SourceEnumerator.Dispose();
        }

        public bool MoveNext()
        {
            return SourceEnumerator.MoveNext();
        }

        public void Reset()
        {
            SourceEnumerator.Reset();
        }
    }
}
