using Lazinator.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Wrappers
{
    public interface ILazinatorHasValue
    {
        [DoNotAutogenerate]
        bool HasValue { get; }
    }
}
