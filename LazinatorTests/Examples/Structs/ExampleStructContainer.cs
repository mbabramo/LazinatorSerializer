using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Wrappers;

namespace LazinatorTests.Examples
{
    public partial class ExampleStructContainer : IExampleStructContainer
    {
        public LazinatorWrapperInt IntWrapper { get; set; }
    }
}
