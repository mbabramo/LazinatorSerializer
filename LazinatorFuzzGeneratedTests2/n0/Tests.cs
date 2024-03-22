
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


namespace FuzzTests.n0
{
    public partial class Tests
    {
        
        
        public void Test0()
        {
            TimeShellClass v1 = new TimeShellClass()
            {
                HappyRecall = 'N',
                TicketImage = null,
                InteractionPassion = 35,
                SufferYear = false
                
            }
            ;
            v1.InteractionPassion = 12147;
            v1.InteractionPassion = -28893;
            Debug.Assert(v1.Equals(new TimeShellClass()
            {
                HappyRecall = 'N',
                TicketImage = null,
                InteractionPassion = -28893,
                SufferYear = false
                
            }
            ));
            
        }
        public void Test1()
        {
            TrueAttachStruct v1 = new TrueAttachStruct()
            {
                BottomOpinion = null,
                PresidentMusic = 123,
                FlowStranger = new RefugeeSmartClass()
                {
                    TicketImage = -203,
                    InteractionPassion = null,
                    SufferYear = true
                    
                }
                ,
                VisitorWooden = new DateTime(2775514310387332270)
                
            }
            ;
            v1.VisitorWooden = new DateTime(3005234299638514626);
            v1 = new TrueAttachStruct()
            {
                BottomOpinion = -56,
                PresidentMusic = null,
                FlowStranger = new RefugeeSmartClass()
                {
                    TicketImage = null,
                    InteractionPassion = null,
                    SufferYear = false
                    
                }
                ,
                VisitorWooden = new DateTime(2229455213319230546)
                
            }
            ;
            Debug.Assert(v1.Equals(new TrueAttachStruct()
            {
                BottomOpinion = -56,
                PresidentMusic = null,
                FlowStranger = new RefugeeSmartClass()
                {
                    TicketImage = null,
                    InteractionPassion = null,
                    SufferYear = false
                    
                }
                ,
                VisitorWooden = new DateTime(2229455213319230546)
                
            }
            ));
            
        }
    }
    
    
    public static class TestRunner
    {
        public static bool RunAllTests()
        {
            var runner = new Tests();
            runner.Test0();
            runner.Test1();
            return true;
        }
        };
        
        
    }
    
    
