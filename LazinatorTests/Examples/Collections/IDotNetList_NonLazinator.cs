﻿using System.Collections.Generic;
using Lazinator.Attributes;

namespace LazinatorTests.Examples.Collections
{
    [Lazinator((int)ExampleUniqueIDs.DotNetList_NonLazinator)]
    public interface IDotNetList_NonLazinator
    {
        List<NonLazinatorClass> MyListNonLazinatorType { get; set; }
        bool MyListNonLazinatorType_Dirty { get; set; }
        List<NonLazinatorClass> MyListNonLazinatorType2 { get; set; }
    }
}