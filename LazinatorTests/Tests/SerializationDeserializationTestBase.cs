using System;
using System.Linq;
using FluentAssertions;
using LazinatorTests.Examples;
using Lazinator.Exceptions;
using Lazinator.Core;
using ExampleNonexclusiveInterfaceImplementer = LazinatorTests.Examples.ExampleNonexclusiveInterfaceImplementer;

namespace LazinatorTests.Tests
{
    public class SerializationDeserializationTestBase
    {

        internal void ChangeHierarchyToGoal(Example copy, Example goal, bool serializeAndDeserializeFirst, bool setDirtyFlag, bool verifyCleanliness)
        {
            var hierarchy = GetHierarchy(1, 1, 1, 1, 0);
            if (serializeAndDeserializeFirst)
            {
                hierarchy = hierarchy.CloneLazinatorTyped();
                ExampleEqual(hierarchy, copy).Should().BeTrue();
            }
            hierarchy.MyNonLazinatorChild.MyString = goal.MyNonLazinatorChild.MyString;
            hierarchy.MyNonLazinatorChild.MyInt = goal.MyNonLazinatorChild.MyInt;
            if (setDirtyFlag)
                hierarchy.MyNonLazinatorChild_Dirty = true;
            bool shouldThrow = serializeAndDeserializeFirst && !setDirtyFlag && verifyCleanliness; // note: If it has not been deserialized first, then it will definitely have to serialize, and so there is no comparison to make
            if (shouldThrow)
            {
                Action act = () => CloneWithOptionalVerification(hierarchy, true, verifyCleanliness);
                act.Should().Throw<UnexpectedDirtinessException>();
            }
            else
            {
                hierarchy = CloneWithOptionalVerification(hierarchy, true, verifyCleanliness);
                bool shouldBeEqual = !serializeAndDeserializeFirst || setDirtyFlag; // if we don't set dirty flag, then either we get an exception or we get the wrong data, unless the data has never been serialized and deserialized
                ExampleEqual(hierarchy, goal).Should().Be(shouldBeEqual);
            }
        }

        internal T CloneWithOptionalVerification<T>(T original, bool includeChildren, bool verifyCleanness) where T : ILazinator, new()
        {
            var bytes = original.SerializeNewBuffer(includeChildren ? IncludeChildrenMode.IncludeAllChildren : IncludeChildrenMode.ExcludeAllChildren, verifyCleanness);
            var result = new T
            {
                HierarchyBytes = bytes,
            };

            return result;
        }


        internal Example GetHierarchy(int indexUpTo2, int indexUpTo3a, int indexUpTo3b, int indexUpTo3c, int indexUpTo2b)
        {
            var parent = GetExample(indexUpTo2);
            parent.MyChild1 = GetExampleChild(indexUpTo3a);
            parent.MyChild2 = GetExampleChild(indexUpTo3b);
            parent.MyNonLazinatorChild = GetNonLazinatorType(indexUpTo3c);
            parent.MyInterfaceImplementer = GetExampleInterfaceImplementer(indexUpTo2b);
            return parent;
        }

        internal Example GetExample(int index)
        {
            if (index == 0)
                return new Example()
                {
                    MyBool = true,
                    MyChar = 'b',
                    MyString = "hello, world",
                    MyUint = (uint) 2342343242,
                    MyNullableDouble = (double)3.5,
                    MyNullableDecimal = (decimal?)-2341.5212352,
                    MyNullableTimeSpan = TimeSpan.FromHours(3),
                    MyDateTime = new DateTime(2000, 1, 1),
                    MyTestEnum = TestEnum.MyTestValue2,
                    MyTestEnumByteNullable = null,
                    WrappedInt = 5

                };
            else if (index == 1)
                return new Example()
                {
                    MyBool = false,
                    MyChar = '\u2342',
                    MyString = "",
                    MyUint = (uint) 1235,
                    MyNullableDouble = (double?)4.2,
                    MyNullableDecimal = null,
                    MyNullableTimeSpan = TimeSpan.FromDays(4),
                    MyDateTime = new DateTime(1972, 10, 22, 17, 36, 0),
                    MyTestEnum = TestEnum.MyTestValue3,
                    MyTestEnumByteNullable = TestEnumByte.MyTestValue,
                    WrappedInt = 2
                };
            else if (index == 2)
                return new Example()
                {
                    MyBool = true,
                    MyChar = '\n',
                    MyString = null,
                    MyUint = (uint) 3127,
                    MyNullableDouble = null,
                    MyNullableDecimal = (decimal?)234243252341,
                    MyTestEnum = TestEnum.MyTestValue3,
                    MyTestEnumByteNullable = TestEnumByte.MyTestValue3
                };
            throw new NotSupportedException();
        }

        internal ExampleChild GetExampleChild(int index)
        {
            if (index == 0)
                return null;
            else if (index == 1)
                return new ExampleChild() {MyLong = 123123, MyShort = 543};
            else if (index == 2)
                return new ExampleChild() {MyLong = 999888, MyShort = -23};
            else if (index == 3)
                return new ExampleChildInherited() {MyLong = 234123, MyInt = 5432, MyShort = 2341}; // the subtype
            throw new NotImplementedException();
        }

        internal IExampleNonexclusiveInterface GetExampleInterfaceImplementer(int index)
        {
            if (index == 0)
                return null;
            else if (index == 1)
                return new ExampleChildInherited() { MyLong = 234123, MyInt = 5432, MyShort = 2341 }; // the subtype
            else if (index == 2)
                return new ExampleNonexclusiveInterfaceImplementer() { MyInt = 27 };
            throw new NotImplementedException();
        }

        internal NonLazinatorClass GetNonLazinatorType(int index)
        {
            if (index == 0)
                return null;
            else if (index == 1)
                return new NonLazinatorClass() {MyInt = 31, MyString = "ok now"};
            else if (index == 2)
                return new NonLazinatorClass() { MyInt = 345, MyString = "" };
            else if (index == 3)
                return new NonLazinatorClass() {MyInt = 876, MyString = "dokey\r\n"};
            throw new NotImplementedException();
        }


        internal bool ExampleEqual(Example example1, Example example2)
        {
            if (example1 == null && example2 == null)
                return true;
            if ((example1 == null) != (example2 == null))
                return false;
            return example1.MyBool == example2.MyBool && example1.MyString == example2.MyString && example1.MyNewString == example2.MyNewString && example1.MyOldString == example2.MyOldString && example1.MyUint == example2.MyUint && example1.MyNullableDouble == example2.MyNullableDouble && example1.MyNullableDecimal == example2.MyNullableDecimal && example1.MyDateTime == example2.MyDateTime && example1.MyNullableTimeSpan == example2.MyNullableTimeSpan && ExampleChildEqual(example1.MyChild1, example2.MyChild1) && ExampleChildEqual(example1.MyChild2, example2.MyChild2) && InterfaceImplementerEqual(example1.MyInterfaceImplementer, example2.MyInterfaceImplementer) && NonLazinatorTypeEqual(example1.MyNonLazinatorChild, example2.MyNonLazinatorChild);
        }

        internal bool ExampleChildEqual(ExampleChild child1, ExampleChild child2)
        {
            if (child1 == null && child2 == null)
                return true;
            if ((child1 == null) != (child2 == null))
                return false;
            if ((child1 is ExampleChildInherited) != (child2 is ExampleChildInherited))
                return false;
            bool basicFieldsEqual = (child1.MyLong == child2.MyLong && child1.MyShort == child2.MyShort && child1.ByteSpan.ToArray().SequenceEqual(child2.ByteSpan.ToArray()));
            bool inheritedFieldsEqual = !(child1 is ExampleChildInherited child1Inherited) || child1Inherited.MyInt == ((ExampleChildInherited) child2).MyInt;
            return basicFieldsEqual && inheritedFieldsEqual;
        }

        internal bool InterfaceImplementerEqual(IExampleNonexclusiveInterface child1, IExampleNonexclusiveInterface child2)
        {
            if (child1 == null && child2 == null)
                return true;
            if ((child1 == null) != (child2 == null))
                return false;
            if ((child1 is ExampleChildInherited) != (child2 is ExampleChildInherited))
                return false;
            bool sharedFieldEqual = (child1.MyInt == child2.MyInt);
            bool otherFieldsEqual = !(child1 is ExampleChildInherited child1Inherited) || (child1Inherited.MyLong == ((ExampleChildInherited)child2).MyLong && child1Inherited.MyShort == ((ExampleChildInherited)child2).MyShort);
            return sharedFieldEqual && otherFieldsEqual;
        }

        internal bool NonLazinatorTypeEqual(NonLazinatorClass instance1, NonLazinatorClass instance2)
        {
            if (instance1 == null && instance2 == null)
                return true;
            if ((instance1 == null) != (instance2 == null))
                return false;
            return instance1.MyString == instance2.MyString && instance1.MyInt == instance2.MyInt;
        }
    }
}
