using System;
using Lazinator.Attributes;

namespace LazinatorTests.Examples
{
    [Lazinator((int)ExampleUniqueIDs.Example, 3, true)]
    public interface IExample
    {
        bool MyBool { get; set; }
        char MyChar { get; set; }
        ExampleChild MyChild1 { get; set; }
        ExampleChild MyChild2 { get; set; }
        [EliminatedWithVersion(3)]
        ExampleChild MyChild2Previous { get; set; }
        DateTime MyDateTime { get; set; }
        IExampleNonexclusiveInterface MyInterfaceImplementer { get; set; }
        [IntroducedWithVersion(3)]
        string MyNewString { get; set; }
        NonLazinatorClass MyNonLazinatorChild { get; set; }
        bool MyNonLazinatorChild_Dirty { get; set; }
        [SetterAccessibility(Accessibility.Internal)]
        decimal? MyNullableDecimal { get; }
        double? MyNullableDouble { get; set; }
        TimeSpan? MyNullableTimeSpan { get; set; }
        [EliminatedWithVersion(3)]
        string MyOldString { get; set; }
        string MyString { get; set; }
        uint MyUint { get; set; }
        TestEnum MyTestEnum { get; set; }
        TestEnumByte? MyTestEnumByteNullable { get; set; }
    }
}