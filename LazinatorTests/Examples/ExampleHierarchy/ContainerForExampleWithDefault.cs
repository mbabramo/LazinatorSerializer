﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples
{
    public partial class ContainerForExampleWithDefault : IContainerForExampleWithDefault
    {
        public ContainerForExampleWithDefault()
        {
            Example = new Example()
            {
                MyChar = 'D'
            };
        }

    }
}
