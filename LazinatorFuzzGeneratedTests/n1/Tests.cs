
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Lazinator.Core;
using Lazinator.Wrappers;
using Lazinator.Collections.Tuples;
using Lazinator.Buffers;
using System.Threading.Tasks;
using Lazinator.Exceptions;
using FluentAssertions;
using Xunit;

namespace FuzzTests.n1
{
    public partial class Tests
    {
        
        
        [Fact]
        public void Test0()
        {
            RefugeeSmartClass v1 = new RefugeeSmartClass()
            {
                TicketImage = null,
                InteractionPassion = 7852,
                SufferYear = true
                
            }
            ;
            v1 = new RefugeeSmartClass()
            {
                TicketImage = 3315790323605508288,
                InteractionPassion = 237,
                SufferYear = false
                
            }
            ;
            v1 = new RefugeeSmartClass()
            {
                TicketImage = 86,
                InteractionPassion = 299,
                SufferYear = true
                
            }
            ;
            Debug.Assert(v1.Equals(new RefugeeSmartClass()
            {
                TicketImage = 86,
                InteractionPassion = 299,
                SufferYear = true
                
            }
            ));
            
        }
        [Fact]
        public void Test1()
        {
            RefugeeSmartClass v1 = new RefugeeSmartClass()
            {
                TicketImage = -582958056139860736,
                InteractionPassion = 19242,
                SufferYear = false
                
            }
            ;
            v1.TicketImage = 5914216517231528227;
            v1 = new RefugeeSmartClass()
            {
                TicketImage = null,
                InteractionPassion = null,
                SufferYear = false
                
            }
            ;
            Debug.Assert(v1.Equals(new RefugeeSmartClass()
            {
                TicketImage = null,
                InteractionPassion = null,
                SufferYear = false
                
            }
            ));
            
        }
    }
    
}


