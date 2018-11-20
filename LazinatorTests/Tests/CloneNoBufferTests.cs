using System;
using System.Collections.Generic;
using System.Linq;
using Lazinator.Collections;
using LazinatorTests.Examples;
using Lazinator.Core;
using Xunit;
using Lazinator.Wrappers;
using LazinatorTests.Examples.Structs;
using LazinatorTests.Examples.NonAbstractGenerics;
using LazinatorTests.Examples.Collections;
using LazinatorTests.Examples.Abstract;
using LazinatorTests.Examples.Subclasses;
using Lazinator.Collections.Dictionary;
using LazinatorTests.Examples.Tuples;

namespace LazinatorTests.Tests
{
    public class CloneNoBufferTests : SerializationDeserializationTestBase
    {
        private void VerifyCloningEquivalence(Func<ILazinator> lazinator)
        {
            VerifyCloningEquivalence(lazinator(), IncludeChildrenMode.ExcludeAllChildren);
            VerifyCloningEquivalence(lazinator(), IncludeChildrenMode.IncludeAllChildren);
            VerifyCloningEquivalence(lazinator(), IncludeChildrenMode.ExcludeOnlyExcludableChildren);
            VerifyCloningEquivalence(lazinator(), IncludeChildrenMode.IncludeOnlyIncludableChildren);
        }

        private void VerifyCloningEquivalence(ILazinator lazinator, IncludeChildrenMode includeChildrenMode)
        {
            var clonedWithBuffer = lazinator.CloneLazinator(includeChildrenMode, CloneBufferOptions.IndependentBuffers);
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
            VerifyCloningEquivalence(() => GetTypicalExample());
        }

        [Fact]
        public void CloneWithoutBuffer_SpanAndMemory()
        {
            VerifyCloningEquivalence(() => LazinatorSpanTests.GetSpanAndMemory(false));
        }

        [Fact]
        public void CloneWithoutBuffer_SpanAndMemory_Empty()
        {
            VerifyCloningEquivalence(() => LazinatorSpanTests.GetSpanAndMemory(true));
        }

        [Fact]
        public void CloneWithoutBuffer_DotNetListValues()
        {
            VerifyCloningEquivalence(() => new DotNetList_Values()
            {
                MyListInt = new List<int>() { 3, 4, 5 }
            });
        }

        [Fact]
        public void CloneWithoutBuffer_DotNetHashSet()
        {
            VerifyCloningEquivalence(() => new DotNetHashSet_Lazinator()
            {
                MyHashSetSerialized = new HashSet<ExampleChild>()
                    {
                        GetExampleChild(1),
                        GetExampleChild(3),
                        null // null item
                    }
            });
        }

        [Fact]
        public void CloneWithoutBuffer_DotNetList_Lazinator()
        {
            VerifyCloningEquivalence(() => new DotNetList_Lazinator()
            {
                MyListSerialized = new List<ExampleChild>()
                    {
                        GetExampleChild(1),
                        GetExampleChild(3), // inherited item
                        null // null item
                    },

            });
        }

        [Fact]
        public void CloneWithoutBuffer_DotNetList_Wrapper()
        {
            VerifyCloningEquivalence(() => new DotNetList_Wrapper()
            {
                MyListNullableByte = new List<WNullableByte>() { 3, 4, 249, null },
                MyListNullableInt = new List<WNullableInt>() { 3, 16000, 249, null, 1000000000 }
            });
        }

        [Fact]
        public void CloneWithoutBuffer_DotNetList_Nested_NonLazinator()
        {
            VerifyCloningEquivalence(() => new DotNetList_Nested_NonLazinator()
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
            });
        }

        [Fact]
        public void CloneWithoutBuffer_DotNetQueue_Lazinator()
        {
            VerifyCloningEquivalence(() => new DotNetQueue_Lazinator()
            {
                MyQueueSerialized = new Queue<ExampleChild>(new[] { new ExampleChild() { MyLong = 3 }, new ExampleChild() { MyLong = 4 }, new ExampleChild() { MyLong = 5 } })
            });
        }

        [Fact]
        public void CloneWithoutBuffer_DotNetStack_Values()
        {
            VerifyCloningEquivalence(() => new DotNetStack_Values()
            {
                MyStackInt = new Stack<int>(new[] { 3, 4, 5 })
            });
        }

        [Fact]
        public void CloneWithoutBuffer_OpenGenericStayingOpenContainer()
        {
            VerifyCloningEquivalence(() => new OpenGenericStayingOpenContainer()
            {
                ClosedGenericFloat =  new OpenGeneric<WFloat>()
                {
                    MyT = new WFloat(3.4F)
                }
            });
        }


        [Fact]
        public void CloneWithoutBuffer_ClosedGenericInterface()
        {
            VerifyCloningEquivalence(() => new OpenGenericStayingOpenContainer()
            {
                ClosedGenericInterface = new OpenGeneric<IExampleChild>()
                {
                    MyT = new ExampleChild() { MyShort = 45 }
                }
            });
        }

        [Fact]
        public void CloneWithoutBuffer_ClosedGenericNonexclusiveInterface()
        {
            VerifyCloningEquivalence(() => new OpenGenericStayingOpenContainer()
            {
                ClosedGenericNonexclusiveInterface = new OpenGeneric<IExampleNonexclusiveInterface>()
                {
                    MyT = new ExampleNonexclusiveInterfaceImplementer() { MyInt = 45 }
                }
            });
        }

        [Fact]
        public void CloneWithoutBuffer_ClosedGenericBase()
        {
            VerifyCloningEquivalence(() => new OpenGenericStayingOpenContainer()
            {
                ClosedGenericBase = new OpenGeneric<Base>()
                {
                    MyT = new Base()
                },
                ClosedGenericFromBaseWithBase = new GenericFromBase<Base>()
                {
                    MyT = new Base()
                }
            });
        }

        [Fact]
        public void CloneWithoutBuffer_ClosedGenericBase_GenericFromBase()
        {
            VerifyCloningEquivalence(() => new OpenGenericStayingOpenContainer()
            {
                ClosedGenericBase = new OpenGeneric<Base>()
                {
                    MyT = new GenericFromBase<WInt>()
                },
                ClosedGenericFromBaseWithBase = new GenericFromBase<Base>()
                {
                    MyT = new GenericFromBase<WInt>()
                }
            });
        }

        [Fact]
        public void CloneWithoutBuffer_ConcreteGeneric2a()
        {
            VerifyCloningEquivalence(() => new ConcreteGeneric2a()
            {
                AnotherProperty = "hi",
                MyT = 5, // now is an int
                LazinatorExample = GetExample(2),
            });
        }

        [Fact]
        public void CloneWithoutBuffer_DerivedGenericContainer()
        {
            VerifyCloningEquivalence(() => new DerivedGenericContainer<WInt>()
            {
                Item = new DerivedGeneric2c<WInt>()
                {
                    MyT = 5 // now is a wrapped int -- note that Item is defined as being IAbstract<T>
                },
            });
        }

        [Fact]
        public void CloneWithoutBuffer_GenericFromBase()
        {
            VerifyCloningEquivalence(() => new GenericFromBase<WInt>()
            {
                MyT = 5
            });
        }

        [Fact]
        public void CloneWithoutBuffer_ClassWithSubclass()
        {
            VerifyCloningEquivalence(() => new ClassWithSubclass()
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
            });
        }

        [Fact]
        public void CloneWithoutBuffer_ConcreteClassesInheritingFromAbstract()
        {
            VerifyCloningEquivalence(() => new Concrete5()
            {
                String1 = "1",
                String2 = "2",
                String3 = "3",
                String4 = "4",
                String5 = "5"
            });
        }

        [Fact]
        public void CloneWithoutBuffer_ContainerWithAbstract1()
        {
            VerifyCloningEquivalence(() => new ContainerWithAbstract1()
            {
                AbstractProperty = new Concrete3() { String1 = "1", String2 = "2", String3 = "3" }
            });
        }

        [Fact]
        public void CloneWithoutBuffer_NonLazinatorContainer()
        {
            VerifyCloningEquivalence(() => new NonLazinatorContainer()
            {
                NonLazinatorClass = new NonLazinatorClass() { MyInt = 5, MyString = "hi" },
                NonLazinatorStruct = new NonLazinatorStruct() { MyInt = 6, MyString = null }
            });
        }

        [Fact]
        public void CloneWithoutBuffer_ContainerForExampleStructWithoutClass()
        {
            VerifyCloningEquivalence(() => new ContainerForExampleStructWithoutClass()
            {
                ExampleStructWithoutClass = new ExampleStructWithoutClass() { MyInt = 3 }
            });
        }

        [Fact]
        public void CloneWithoutBuffer_ExampleStructContainingStruct()
        {
            VerifyCloningEquivalence(() => new ExampleStructContainingStruct()
            {
                MyExampleStructContainingClasses = new ExampleStructContainingClasses() { MyChar = 'z', MyLazinatorList = new List<Example>() }
            });
        }

        [Fact]
        public void CloneWithoutBuffer_ExampleStruct()
        {
            ExampleStructContainingClasses GetExampleStruct()
            {
                ExampleStructContainingClasses s = new ExampleStructContainingClasses();
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
                return s;
            }
            VerifyCloningEquivalence(() => GetExampleStruct());
        }

        [Fact]
        public void CloneWithoutBuffer_WReadOnlySpanChar()
        {
            WReadOnlySpanChar GetWReadOnlySpanChar()
            {
                WReadOnlySpanChar wReadOnlySpanChar = new WReadOnlySpanChar();
                wReadOnlySpanChar.Value = new Span<char>("mystring".ToArray());
                return wReadOnlySpanChar;
            }
            VerifyCloningEquivalence(() => GetWReadOnlySpanChar());
        }

        [Fact]
        public void CloneWithoutBuffer_LazinatorList_Example()
        {
            VerifyCloningEquivalence(() => new LazinatorList<Example>() { GetExample(1), GetExample(1) });
        }

        [Fact]
        public void CloneWithoutBuffer_LazinatorList_WInt()
        {
            VerifyCloningEquivalence(() => new LazinatorList<WInt>() { 3 });
        }

        [Fact]
        public void CloneWithoutBuffer_LazinatorArray_Example()
        {
            LazinatorArray<Example> GetArray()
            {
                LazinatorArray<Example> l = new LazinatorArray<Example>(3);
                l[0] = GetExample(1);
                l[1] = GetExample(1);
                l[2] = null;
                return l;
            }
            VerifyCloningEquivalence(() => GetArray());
        }

        [Fact]
        public void CloneWithoutBuffer_LazinatorDictionary()
        {
            LazinatorDictionary<WInt, Example> GetDictionary()
            {
                LazinatorDictionary<WInt, Example> d = new LazinatorDictionary<WInt, Example>();
                d[23] = GetExample(1);
                d[0] = GetExample(2);
                return d;
            }
            VerifyCloningEquivalence(() => GetDictionary());
        }

        [Fact]
        public void CloneWithoutBuffer_StructTuple()
        {
            StructTuple GetObject()
            {
                return new StructTuple()
                {
                    MyNullableTuple = (3, 4)
                };
            }
            VerifyCloningEquivalence(() => GetObject());
        }

        [Fact]
        public void CloneWithoutBuffer_StructTuple_Null()
        {
            StructTuple GetObject()
            {
                return new StructTuple()
                {
                    MyNullableTuple = null
                };
            }
            VerifyCloningEquivalence(() => GetObject());
        }

        [Fact]
        public void CloneWithoutBuffer_RegularTuple()
        {
            RegularTuple GetObject()
            {
                return new RegularTuple()
                {
                    MyTupleSerialized = new Tuple<uint, ExampleChild, NonLazinatorClass>(3, GetExampleChild(1),
                        GetNonLazinatorType(2))
                };
            }
            VerifyCloningEquivalence(() => GetObject());
        }

        [Fact]
        public void CloneWithoutBuffer_RegularTuple_Null()
        {
            RegularTuple GetObject()
            {
                return new RegularTuple()
                {
                    MyTupleSerialized = null
                };
            }
            VerifyCloningEquivalence(() => GetObject());
        }
    }
}
