
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentAssertions;
using Lazinator.Core;
using Xunit;
using Lazinator.Wrappers;
using Lazinator.Collections.Tuples;
using Lazinator.Buffers;
using System.Threading.Tasks;
using Lazinator.Exceptions;

namespace FuzzTests.n1
{
    public partial class Tests
    {
        
        
        [Fact]
        public void Test0()
        {
            RefugeeSmartStruct v1 = new RefugeeSmartStruct()
            {
                TicketImage = 200,
                InteractionPassion = 17333
                
            }
            ;
            v1.TicketImage = 5914216517231528227;
            v1 = new RefugeeSmartStruct()
            {
                TicketImage = null,
                InteractionPassion = null
                
            }
            ;
            v1.InteractionPassion = -18263;
            v1.TicketImage = -7250089089678572612;
            v1.InteractionPassion = 106;
            Debug.Assert(v1.Equals(new RefugeeSmartStruct()
            {
                TicketImage = -7250089089678572612,
                InteractionPassion = 106
                
            }
            ));
            
        }
        
    }
}


