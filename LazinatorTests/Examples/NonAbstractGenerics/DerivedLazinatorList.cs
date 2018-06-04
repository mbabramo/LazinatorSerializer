﻿using Lazinator.Attributes;
using Lazinator.Collections;
using Lazinator.Support;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples
{
    public partial class DerivedLazinatorList<T> : LazinatorList<T>, IDerivedLazinatorList<T> where T : ILazinator, new()
    {
        // We must override these methods, so that the code behind knows that they are defined in this class. Otherwise, it will not call them. 

        public override void PreSerialization()
        {
            base.PreSerialization();
        }

        public override void MarkHierarchyClean()
        {
            base.MarkHierarchyClean();
        }
    }
}
