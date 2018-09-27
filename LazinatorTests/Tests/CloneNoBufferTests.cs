using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Lazinator.Collections;
using LazinatorTests.Examples;
using Lazinator.Core;
using Xunit;
using Lazinator.Wrappers;
using LazinatorTests.Examples.Hierarchy;
using LazinatorTests.Examples.Structs;
using LazinatorTests.Examples.NonAbstractGenerics;
using LazinatorTests.Examples.Collections;
using LazinatorTests.Examples.Abstract;
using LazinatorTests.Examples.Subclasses;

namespace LazinatorTests.Tests
{
    public class CloneNoBufferTests : SerializationDeserializationTestBase
    {
        private void VerifyCloningEquivalence(ILazinator lazinator)
        {
            VerifyCloningEquivalence(lazinator, IncludeChildrenMode.IncludeAllChildren);
            VerifyCloningEquivalence(lazinator, IncludeChildrenMode.ExcludeAllChildren);
            VerifyCloningEquivalence(lazinator, IncludeChildrenMode.ExcludeOnlyExcludableChildren); // DEBUG -- fix order
            VerifyCloningEquivalence(lazinator, IncludeChildrenMode.IncludeOnlyIncludableChildren);
        }

        private void VerifyCloningEquivalence(ILazinator lazinator, IncludeChildrenMode includeChildrenMode)
        {
            var clonedWithBuffer = lazinator.CloneLazinator(includeChildrenMode, CloneBufferOptions.LinkedBuffer);
            var clonedNoBuffer = lazinator.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
            var clonedWithBufferString = new HierarchyTree(clonedWithBuffer).ToString();
            var clonedNoBufferString = new HierarchyTree(clonedNoBuffer).ToString();
            try
            {
                LazinatorUtilities.ConfirmHierarchiesEqual(clonedWithBuffer, clonedNoBuffer);
            }
            catch (Exception ex)
            {
                int i = 0;
                if (clonedNoBuffer.IsStruct)
                    clonedNoBuffer = clonedNoBuffer.CloneLazinator();
                for (; i < Math.Min(clonedWithBuffer.LazinatorMemoryStorage.Span.Length, clonedNoBuffer.LazinatorMemoryStorage.Span.Length); i++)
                    if (clonedWithBuffer.LazinatorMemoryStorage.Span[i] != clonedNoBuffer.LazinatorMemoryStorage.Span[i])
                    {
                        break;
                    }
                throw new Exception("Verify cloning failed at position " + i + ". See inner exception.", ex);
            }
        }

        [Fact]
        public void CloneWithoutBuffer_Example()
        {
            VerifyCloningEquivalence(GetTypicalExample());
        }

        [Fact]
        public void CloneWithoutBuffer_SpanAndMemory()
        {
            SpanAndMemory s = LazinatorSpanTests.GetSpanAndMemory(false);
            VerifyCloningEquivalence(s);
        }

        [Fact]
        public void CloneWithoutBuffer_SpanAndMemory_Empty()
        {
            SpanAndMemory s = LazinatorSpanTests.GetSpanAndMemory(true);
            VerifyCloningEquivalence(s);
        }

        [Fact]
        public void CloneWithoutBuffer_DotNetListValues()
        {
            DotNetList_Values d = new DotNetList_Values()
                {
                    MyListInt = new List<int>() { 3, 4, 5 }
                };
            VerifyCloningEquivalence(d);
        }

        [Fact]
        public void CloneWithoutBuffer_DotNetHashSet()
        {
            DotNetHashSet_Lazinator d = new DotNetHashSet_Lazinator()
            {
                MyHashSetSerialized = new HashSet<ExampleChild>()
                    {
                        GetExampleChild(1),
                        GetExampleChild(3), 
                        null // null item
                    }
            };
            VerifyCloningEquivalence(d);
        }

        [Fact]
        public void CloneWithoutBuffer_DotNetList_Lazinator()
        {
            DotNetList_Lazinator d = new DotNetList_Lazinator()
            {
                MyListSerialized = new List<ExampleChild>()
                    {
                        GetExampleChild(1),
                        GetExampleChild(3), // inherited item
                        null // null item
                    },

            };
            VerifyCloningEquivalence(d);
        }

        [Fact]
        public void CloneWithoutBuffer_DotNetList_Wrapper()
        {
            DotNetList_Wrapper d = new DotNetList_Wrapper()
            {
                MyListNullableByte = new List<WNullableByte>() { 3, 4, 249, null },
                MyListNullableInt = new List<WNullableInt>() { 3, 16000, 249, null, 1000000000 }
            };
            VerifyCloningEquivalence(d);
        }

        [Fact]
        public void CloneWithoutBuffer_DotNetList_Nested_NonLazinator()
        {
            DotNetList_Nested_NonLazinator d = new DotNetList_Nested_NonLazinator()
            {
                MyListNestedNonLazinatorType = new List<List<NonLazinatorClass>>()
                    {
                        new List<NonLazinatorClass>()
                        {
                            GetNonLazinatorType(1),
                            null,
                            GetNonLazinatorType(3),
                        },
                        new List<NonLazinatorClass>()
                        {
                            GetNonLazinatorType(2),
                            GetNonLazinatorType(1),
                        },
                        new List<NonLazinatorClass>()
                        {
                        },
                        null // null item
                    }
            };
            VerifyCloningEquivalence(d);
        }

        [Fact]
        public void CloneWithoutBuffer_DotNetQueue_Lazinator()
        {
            DotNetQueue_Lazinator d = new DotNetQueue_Lazinator()
            {
                MyQueueSerialized = new Queue<ExampleChild>(new[] { new ExampleChild() { MyLong = 3 }, new ExampleChild() { MyLong = 4 }, new ExampleChild() { MyLong = 5 } })
            };
            VerifyCloningEquivalence(d);
        }

        [Fact]
        public void CloneWithoutBuffer_DotNetStack_Values()
        {
            DotNetStack_Values d = new DotNetStack_Values()
            {
                MyStackInt = new Stack<int>(new[] { 3, 4, 5 })
            };
            VerifyCloningEquivalence(d);
        }

        [Fact]
        public void CloneWithoutBuffer_OpenGenericStayingOpenContainer()
        {
            OpenGenericStayingOpenContainer d = new OpenGenericStayingOpenContainer()
            {
                ClosedGenericFloat = new OpenGeneric<WFloat>()
                {
                    MyT = new WFloat(3.4F)
                }
            };
            VerifyCloningEquivalence(d);
        }


        [Fact]
        public void CloneWithoutBuffer_ClosedGenericInterface()
        {
            OpenGenericStayingOpenContainer x = new OpenGenericStayingOpenContainer()
            {
                ClosedGenericInterface = new OpenGeneric<IExampleChild>()
                {
                    MyT = new ExampleChild() { MyShort = 45 }
                }
            };
            VerifyCloningEquivalence(x);
        }

        [Fact]
        public void CloneWithoutBuffer_ClosedGenericNonexclusiveInterface()
        {
            OpenGenericStayingOpenContainer d = new OpenGenericStayingOpenContainer()
            {
                ClosedGenericNonexclusiveInterface = new OpenGeneric<IExampleNonexclusiveInterface>()
                {
                    MyT = new ExampleNonexclusiveInterfaceImplementer() { MyInt = 45 }
                }
            };
            VerifyCloningEquivalence(d);
        }

        [Fact]
        public void CloneWithoutBuffer_ClosedGenericBase()
        {
            OpenGenericStayingOpenContainer d = new OpenGenericStayingOpenContainer()
            {
                ClosedGenericBase = new OpenGeneric<Base>()
                {
                    MyT = new Base()
                },
                ClosedGenericFromBaseWithBase = new GenericFromBase<Base>()
                {
                    MyT = new Base()
                }
            };
            VerifyCloningEquivalence(d);
        }

        [Fact]
        public void CloneWithoutBuffer_ClosedGenericBase_GenericFromBase()
        {
            OpenGenericStayingOpenContainer x = new OpenGenericStayingOpenContainer()
            {
                ClosedGenericBase = new OpenGeneric<Base>()
                {
                    MyT = new GenericFromBase<WInt>()
                },
                ClosedGenericFromBaseWithBase = new GenericFromBase<Base>()
                {
                    MyT = new GenericFromBase<WInt>()
                }
            };
            VerifyCloningEquivalence(x);
        }

        [Fact]
        public void CloneWithoutBuffer_ConcreteGeneric2a()
        {
            ConcreteGeneric2a d = new ConcreteGeneric2a()
            {
                AnotherProperty = "hi",
                MyT = 5, // now is an int
                LazinatorExample = GetExample(2),
            };
            VerifyCloningEquivalence(d);
        }

        [Fact]
        public void CloneWithoutBuffer_DerivedGenericContainer()
        {
            DerivedGenericContainer<WInt> d = new DerivedGenericContainer<WInt>()
            {
                Item = new DerivedGeneric2c<WInt>()
                {
                    MyT = 5 // now is a wrapped int -- note that Item is defined as being IAbstract<T>
                },
            };
            VerifyCloningEquivalence(d);
        }

        [Fact]
        public void CloneWithoutBuffer_GenericFromBase()
        {
            GenericFromBase<WInt> d = new GenericFromBase<WInt>()
            {
                MyT = 5
            };
            VerifyCloningEquivalence(d);
        }

        [Fact]
        public void CloneWithoutBuffer_ClassWithSubclass()
        {
            ClassWithSubclass d = new ClassWithSubclass()
            {
                IntWithinSuperclass = 5,
                SubclassInstance1 = new ClassWithSubclass.SubclassWithinClass()
                {
                    StringWithinSubclass = "within1"
                },
                SubclassInstance2 = new ClassWithSubclass.SubclassWithinClass()
                {
                    StringWithinSubclass = "within2"
                }
            };
            VerifyCloningEquivalence(d);
        }

        [Fact]
        public void CloneWithoutBuffer_ConcreteClassesInheritingFromAbstract()
        {
            Concrete5 c = new Concrete5()
            {
                String1 = "1",
                String2 = "2",
                String3 = "3",
                String4 = "4",
                String5 = "5"
            };
            VerifyCloningEquivalence(c);
        }

        [Fact]
        public void CloneWithoutBuffer_ContainerWithAbstract1()
        {
            ContainerWithAbstract1 c = new ContainerWithAbstract1()
            {
                AbstractProperty = new Concrete3() { String1 = "1", String2 = "2", String3 = "3" }
            };
            VerifyCloningEquivalence(c);
        }

        [Fact]
        public void CloneWithoutBuffer_NonLazinatorContainer()
        {
            NonLazinatorContainer c = new NonLazinatorContainer()
            {
                NonLazinatorClass = new NonLazinatorClass() { MyInt = 5, MyString = "hi" },
                NonLazinatorStruct = new NonLazinatorStruct() { MyInt = 6, MyString = null }
            };
            VerifyCloningEquivalence(c);
        }

        [Fact]
        public void CloneWithoutBuffer_ContainerForExampleStructWithoutClass()
        {
            ContainerForExampleStructWithoutClass c = new ContainerForExampleStructWithoutClass()
            {
                ExampleStructWithoutClass = new ExampleStructWithoutClass() { MyInt = 3 }
            };
            VerifyCloningEquivalence(c);
        }

        [Fact]
        public void CloneWithoutBuffer_ExampleStructContainingStruct()
        {
            ExampleStructContainingStruct c = new ExampleStructContainingStruct()
            {
                MyExampleStruct = new ExampleStruct() { MyChar = 'z', MyLazinatorList = new List<Example>() }
            };
            VerifyCloningEquivalence(c);
        }

        [Fact]
        public void CloneWithoutBuffer_ExampleStruct()
        {
            ExampleStruct s = new ExampleStruct();
            s.MyBool = true;
            s.MyChar = 'x';
            s.MyChild1 = new ExampleChildInherited()
            {
                MyInt = 34,
                MyLong = 341341
            };
            s.MyListValues = new List<int>() { 3, 4 };
            s.MyTuple = (new NonLazinatorClass() { MyInt = 5 }, 4);
            s.MyLazinatorList = new List<Example>() { new Example() };
            VerifyCloningEquivalence(s);
        }

        [Fact]
        public void CloneWithoutBuffer_WReadOnlySpanChar()
        {
            WReadOnlySpanChar wReadOnlySpanChar = new WReadOnlySpanChar();
            wReadOnlySpanChar.Value = new Span<char>("mystring".ToArray());
            VerifyCloningEquivalence(wReadOnlySpanChar);
        }

        [Fact]
        public void CloneWithoutBuffer_LazinatorList_Example()
        {
            LazinatorList<Example> l = new LazinatorList<Example>() { GetExample(1), GetExample(1) };
            VerifyCloningEquivalence(l);
        }

        [Fact]
        public void CloneWithoutBuffer_LazinatorList_WInt()
        {
            LazinatorList<WInt> l = new LazinatorList<WInt>() { 3 };
            VerifyCloningEquivalence(l);
        }
    }
}
