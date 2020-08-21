using System;
using System.Collections.Generic;
using System.Linq;
using LazinatorCollections;
using LazinatorTests.Examples;
using Lazinator.Core;
using Xunit;
using Lazinator.Wrappers;
using LazinatorTests.Examples.Structs;
using LazinatorTests.Examples.NonAbstractGenerics;
using LazinatorTests.Examples.Collections;
using LazinatorTests.Examples.Abstract;
using LazinatorTests.Examples.Subclasses;
using LazinatorCollections.Dictionary;
using LazinatorTests.Examples.Tuples;
using LazinatorTests.Examples.ExampleHierarchy;
using FluentAssertions;
using Lazinator.Exceptions;
using System.Threading;
using Lazinator.CodeDescription;

namespace LazinatorTests.Tests
{
    public class CloneNoBufferTests : SerializationDeserializationTestBase
    {
        private void VerifyCloningEquivalence(Func<ILazinator> lazinator)
        {
            VerifyCloningEquivalence(lazinator(), IncludeChildrenMode.IncludeAllChildren);
            VerifyCloningEquivalence(lazinator(), IncludeChildrenMode.ExcludeAllChildren);
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

        public static SpanAndMemory GetSpanAndMemoryNullSpans()
        {

            return new SpanAndMemory
            {
                MyReadOnlyMemoryByte = new Memory<byte>(new byte[] { 3, 4, 5 }),
                MyReadOnlyMemoryChar = new ReadOnlyMemory<char>(LazinatorSpanTests.chars),
                MyReadOnlyMemoryInt = new ReadOnlyMemory<int>(new int[] { 3, 4, 5 }),
                MyReadOnlySpanByte = new Span<byte>(new byte[] { 3, 4, 5 }),
                MyReadOnlySpanChar = new ReadOnlySpan<char>(LazinatorSpanTests.chars),
                MyReadOnlySpanLong = new Span<long>(new long[] { -234234, long.MaxValue }),
                MyMemoryByte = new Memory<byte>(new byte[] { 3, 4, 5 }),
                MyMemoryInt = new Memory<int>(new int[] { 3, 4, 5 }),
                MyNullableMemoryByte = null,
                MyNullableMemoryInt = null,
                MyNullableReadOnlyMemoryInt = null
            };
        }

        [Fact]
        public void CloneWithoutBuffer_SpanAndMemory_NullSpans()
        {
            VerifyCloningEquivalence(() => GetSpanAndMemoryNullSpans());
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
        public void CloneWithoutBuffer_ContainerForExampleStructWithoutClass_WithNullableStruct()
        {
            VerifyCloningEquivalence(() => new ContainerForExampleStructWithoutClass()
            {
                ExampleStructWithoutClass = new ExampleStructWithoutClass() { MyInt = 3 },
                ExampleNullableStruct = new ExampleStructWithoutClass() { MyInt = 4 }
            });
        }

        [Fact]
        public void CloneWithoutBuffer_ContainerForExampleStructWithoutClass_WithNullableStruct_Null()
        {
            VerifyCloningEquivalence(() => new ContainerForExampleStructWithoutClass()
            {
                ExampleStructWithoutClass = new ExampleStructWithoutClass() { MyInt = 3 },
                ExampleNullableStruct = null
            });
        }

        [Fact]
        public void CloneWithoutBuffer_ExampleStructContainingStruct()
        {
            VerifyCloningEquivalence(() => new ExampleStructContainingStruct()
            {
                MyExampleStructContainingClasses = new ExampleStructContainingClasses() { MyChar = 'z', MyLazinatorList = new List<Example>() },
                MyExampleNullableStruct = new ExampleStructContainingClasses() { MyChar = 'a' }
            });
        }

        [Fact]
        public void CloneWithoutBuffer_ExampleStructContainingStruct_WithNullNullableStruct()
        {
            VerifyCloningEquivalence(() => new ExampleStructContainingStruct()
            {
                MyExampleStructContainingClasses = new ExampleStructContainingClasses() { MyChar = 'z', MyLazinatorList = new List<Example>() },
                MyExampleNullableStruct = null
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
        public void CloneWithoutBuffer_StructTupleWithStructs()
        {
            StructTuple GetObject()
            {
                return new StructTuple()
                {
                    MyValueTupleStructs = (3, 4) // wint, wint
                };
            }
            VerifyCloningEquivalence(() => GetObject());
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


        public static NullableEnabledContext GetNullableEnabledContext(bool setNullsWherePossible = false)
        {
            if (!setNullsWherePossible)
            return new NullableEnabledContext(IncludeChildrenMode.IncludeAllChildren,
                 new Example[] { new Example(), new Example() },
                 new Example[] { new Example(), new Example() }, new Example(), new Dictionary<int, Example>() { { 4, new Example() } }, new Dictionary<int, Example>() { { 4, new Example() } }, new Example(), new LazinatorList<Example>() { new Example() }, new LazinatorList<Example>() { new Example() }, new List<Example>() { new Example() }, new List<Example>() { new Example(), new Example() }, new Queue<Example>(), new Queue<Example>(), new RecordLikeClass(3, new Example()), new RecordLikeStruct(4, "hello"), new Tuple<Example, int>(new Example(), 6), new Tuple<Example, int>(new Example(), 6), new Stack<Example>(), new Stack<Example>())
            {
                ExplicitlyNullable = new Example(),
                ExplicitlyNullableInterface = new Example(),

                NullableArrayOfNonNullables = new Example[] { new Example(), new Example() },
                NullableArrayOfNullables = new Example[] { new Example(), new Example() },
                NullableListOfNonNullables = new List<Example>() { new Example(), new Example() },
                NullableListOfNullables = new List<Example>() { new Example(), new Example() },

                ValueTupleWithNonNullable = (new Example(), 5),
                ValueTupleWithNullable = (new Example(), 5),
                NullableValueTupleWithNonNullable = (new Example(), 5),
                NullableValueTupleWithNullable = (new Example(), 5),
                NullableRegularTupleWithNonNullable = new Tuple<Example, int>(new Example(), 6),
                NullableRegularTupleWithNullable = new Tuple<Example, int>(null, 6),

                NullableDictionaryWithNonNullable = new Dictionary<int, Example>() { { 4, new Example() } },
                NullableDictionaryWithNullable = new Dictionary<int, Example>() { { 4, new Example() } },
                NullableLazinatorListNonNullable = new LazinatorList<Example>() { new Example() },
                NullableLazinatorListNullable = new LazinatorList<Example>() { new Example() },
                NullableString = "hello",
                NonNullableString = "world",
                ByteReadOnlySpan = new byte[] { 3, 4 },
                NonNullableMemoryOfBytes = new byte[] { 3, 4 },
                NonNullableReadOnlyMemoryOfBytes = new byte[] { 3, 4 },
                NullableMemoryOfBytes = new byte[] { 3, 4 },
                NullableReadOnlyMemoryOfBytes = new byte[] { 3, 4 },
                NullableQueueOfNonNullables = new Queue<Example>(),
                NullableQueueOfNullables = new Queue<Example>(),
                NullableStackOfNonNullables = new Stack<Example>(),
                NullableStackOfNullables = new Stack<Example>(),

                MyInt = 3, 
                MyNullableInt = 5,

                NonNullableStruct = default,
                NullableStruct = null

            };
            else
                return new NullableEnabledContext(IncludeChildrenMode.IncludeAllChildren,
                 new Example[] { new Example(), new Example() },
                 new Example[] { new Example(), new Example() }, new Example(), new Dictionary<int, Example>() { { 4, new Example() } }, new Dictionary<int, Example>() { { 4, new Example() } }, new Example(), new LazinatorList<Example>() { new Example() }, new LazinatorList<Example>() { new Example() }, new List<Example>() { new Example() }, new List<Example>() { new Example(), new Example() }, new Queue<Example>(), new Queue<Example>(), new RecordLikeClass(3, new Example()), new RecordLikeStruct(4, "hello"), new Tuple<Example, int>(new Example(), 6), new Tuple<Example, int>(new Example(), 6), new Stack<Example>(), new Stack<Example>())
                {
                    ExplicitlyNullable = null,
                    ExplicitlyNullableInterface = null,

                    NullableArrayOfNonNullables = null,
                    NullableArrayOfNullables = null,
                    NullableListOfNonNullables = null,
                    NullableListOfNullables = null,

                    ValueTupleWithNonNullable = (new Example(), 5),
                    ValueTupleWithNullable = (null, 5),
                    NullableValueTupleWithNonNullable = null,
                    NullableValueTupleWithNullable = null,
                    NullableRegularTupleWithNonNullable = null,
                    NullableRegularTupleWithNullable = null,

                    NullableDictionaryWithNonNullable = null,
                    NullableDictionaryWithNullable = null,
                    NullableLazinatorListNonNullable = null,
                    NullableLazinatorListNullable = null,
                    NullableString = null,
                    NonNullableString = "world",
                    ByteReadOnlySpan = new byte[] { 3, 4 },
                    NonNullableMemoryOfBytes = new byte[] { 3, 4 },
                    NonNullableReadOnlyMemoryOfBytes = new byte[] { 3, 4 },
                    NullableMemoryOfBytes = null,
                    NullableReadOnlyMemoryOfBytes = null,
                    NullableQueueOfNonNullables = null,
                    NullableQueueOfNullables = null,
                    NullableStackOfNonNullables = null,
                    NullableStackOfNullables = null,

                    MyInt = 3,
                    MyNullableInt = 5,

                    NonNullableStruct = default,
                    NullableStruct = null

                };

        }

        [Fact]
        public void CloneWithoutBuffer_NullableEnabledContext()
        {

            VerifyCloningEquivalence(GetNullableEnabledContext(), IncludeChildrenMode.IncludeAllChildren);
            VerifyCloningEquivalence(GetNullableEnabledContext(), IncludeChildrenMode.ExcludeOnlyExcludableChildren);
        }

        [Fact]
        public void CloneWithoutBuffer_LazinatorList_NullableEnabledContext()
        {

            VerifyCloningEquivalence(() => new LazinatorList<NullableEnabledContext>() { GetNullableEnabledContext(), null, GetNullableEnabledContext() });

#nullable enable
            VerifyCloningEquivalence(() => new LazinatorList<NullableEnabledContext>() { GetNullableEnabledContext(), GetNullableEnabledContext() });
            var list = new LazinatorList<NullableEnabledContext>() { GetNullableEnabledContext(), GetNullableEnabledContext() };
            NullableEnabledContext item = list.First(); // Note that we are guaranteed that this will produce a nonnullable item.
#nullable disable
        }

        [Fact]
        public void CloneWithoutBuffer_NullableEnabledContext_ThrowsAfterExcludingThenAccessingNonNullable()
        {
            if (PropertyDescription.UseNullableBackingFieldsForNonNullableReferenceTypes)
            {
                Action a = () => { VerifyCloningEquivalence(GetNullableEnabledContext(), IncludeChildrenMode.ExcludeAllChildren); };
                a.Should().Throw<UnsetNonnullableLazinatorException>();
                a = () => { VerifyCloningEquivalence(GetNullableEnabledContext(), IncludeChildrenMode.IncludeOnlyIncludableChildren); };
                a.Should().Throw<UnsetNonnullableLazinatorException>();
            }
            else
            {
                // should not throw
                VerifyCloningEquivalence(GetNullableEnabledContext(), IncludeChildrenMode.ExcludeAllChildren); 
                VerifyCloningEquivalence(GetNullableEnabledContext(), IncludeChildrenMode.IncludeOnlyIncludableChildren);
            }
        }

        [Fact]
        public void CloneWithoutBuffer_NullableEnabledContext_NullablesNull()
        {
            VerifyCloningEquivalence(GetNullableEnabledContext(true), IncludeChildrenMode.IncludeAllChildren);
            VerifyCloningEquivalence(GetNullableEnabledContext(true), IncludeChildrenMode.ExcludeOnlyExcludableChildren);
        }

        [Fact]
        public void CloneWithoutBuffer_NullableEnabledContext_SetNonNullablesToNullThrows()
        {
            NullableEnabledContext GetObject()
            {
                return new NullableEnabledContext(IncludeChildrenMode.IncludeAllChildren,
                 null,
                 new Example[] { new Example(), new Example() }, new Example(), new Dictionary<int, Example>() { { 4, new Example() } }, new Dictionary<int, Example>() { { 4, new Example() } }, new Example(), new LazinatorList<Example>() { new Example() }, new LazinatorList<Example>() { new Example() }, new List<Example>() { new Example() }, new List<Example>() { new Example(), new Example() }, new Queue<Example>(), new Queue<Example>(), new RecordLikeClass(3, new Example()), new RecordLikeStruct(4, "hello"), new Tuple<Example, int>(new Example(), 6), new Tuple<Example, int>(new Example(), 6), new Stack<Example>(), new Stack<Example>());
            }

            Action a = () => { _ = GetObject(); };
            a.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CloneWithoutBuffer_DotNetListNullableEnabledContext()
        {
            DotNetList_Values GetList()
            {
                return new DotNetList_Values()
                {
                    MyListNullableEnabledContext = new List<NullableEnabledContext>() { GetNullableEnabledContext(), null, GetNullableEnabledContext() }
                };
            };

            VerifyCloningEquivalence(GetList(), IncludeChildrenMode.IncludeAllChildren);
            VerifyCloningEquivalence(GetList(), IncludeChildrenMode.ExcludeOnlyExcludableChildren);
        }
    }
}
