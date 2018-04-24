using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;

namespace LazinatorTests.Examples
{
    [Lazinator((int) ExampleUniqueIDs.NonLazinatorContainer)]
    public interface INonLazinatorContainer
    {
        NonLazinatorClass NonLazinatorClass
        {
            get;
            set;
        }
        NonLazinatorStruct NonLazinatorStruct
        {
            get;
            set;
        }
    }
}
