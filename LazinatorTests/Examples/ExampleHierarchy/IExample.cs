using System;
using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Wrappers;
using LazinatorTests.Examples;

namespace LazinatorTests.Examples
{
    [Lazinator((int) ExampleUniqueIDs.Example, 3, true)]
    public interface IExample : ILazinator // note that this ILazinator reference is unnecessary
    {
        bool MyBool { get; set; }
        char MyChar { get; set; }
        ExampleChild MyChild1 { get; set; }
        ExampleChild MyChild2 { get; set; }
        [EliminatedWithVersion(3)] ExampleChild MyChild2Previous { get; set; }
        [InsertAttribute("Newtonsoft.Json.JsonProperty(\"MyDT\")")] DateTime MyDateTime { get; set; }
        IExampleNonexclusiveInterface MyInterfaceImplementer { get; set; }
        [IntroducedWithVersion(3)] string MyNewString { get; set; }
        NonLazinatorClass MyNonLazinatorChild { get; set; }
        bool MyNonLazinatorChild_Dirty { get; set; }
        [SetterAccessibility("internal")] decimal? MyNullableDecimal { get; }
        [DerivationKeyword("virtual")] double? MyNullableDouble { get; set; }
        TimeSpan? MyNullableTimeSpan { get; set; }
        [EliminatedWithVersion(3)] string MyOldString { get; set; }
        string MyString { get; set; }
        uint MyUint { get; set; }
        TestEnum MyTestEnum { get; set; }
        TestEnumByte? MyTestEnumByteNullable { get; set; }
        WInt WrappedInt { get; set; }
        [ExcludableChild] ExampleChild ExcludableChild { get; set; }
        [IncludableChild] ExampleChild IncludableChild { get; set; }
        [AutoChangeParent] ExampleChild MyAutoChangeParentChild { get; set; }
        ExampleStruct MyAutoChangeParentChildStruct { get; set; } // autoChangeParent is automatic for structs
    }
}