﻿using System;
using Lazinator.Buffers;
using Lazinator.Core;
using Lazinator.Wrappers;
using static Lazinator.Core.LazinatorUtilities;

namespace LazinatorTests.Examples
{
    public partial class Example : IExample
    {

        public ExampleChild MyAutocloneChild { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ExampleStruct MyAutocloneChildStruct { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Example()
        {

        }

        public void LazinatorObjectVersionUpgrade(int oldFormatVersion)
        {
            if (oldFormatVersion < 3 && LazinatorObjectVersion >= 3)
            {
                MyNewString = "NEW " + MyOldString;
                MyOldString = null;
            }
        }

        public bool _OnDirtyCalled = false;

        public void OnDirty()
        {
            _OnDirtyCalled = true;
        }
    }
}
