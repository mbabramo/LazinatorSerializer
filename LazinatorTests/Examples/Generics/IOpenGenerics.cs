using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Support;
using Lazinator.Core;

namespace LazinatorTests.Examples
{
    public interface IOpenGenerics<T, U> where U : ILazinator
    {
        T ItemT { get; set; }
        U ItemU { get; set; }
    }
}
