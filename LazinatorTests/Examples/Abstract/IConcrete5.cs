﻿using Lazinator.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Collections;

namespace LazinatorTests.Examples.Abstract
{
    [Lazinator((int)ExampleUniqueIDs.Concrete5)]
    interface IConcrete5
    {
        string String5 { get; set; }
    }
}
