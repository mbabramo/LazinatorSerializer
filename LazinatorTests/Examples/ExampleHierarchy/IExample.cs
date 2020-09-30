using System;
using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace LazinatorTests.Examples
{
    [Lazinator((int) ExampleUniqueIDs.Example, 3, true)]
    public interface IExample : ILazinator // note that this ILazinator reference is unnecessary
    {
        bool MyBool { get; set; }
        [IncludeRefProperty]
        char MyChar { get; set; }
        [OnDeserialized("OnMyChild1Deserialized(_MyChild1);")]
        [OnPropertyAccessed("OnMyChild1Accessed(_MyChild1);")]
        ExampleChild MyChild1 { get; set; }
        ExampleChild MyChild2 { get; set; }
        [EliminatedWithVersion(3)] ExampleChild MyChild2Previous { get; set; }
        [InsertAttribute("Newtonsoft.Json.JsonProperty(\"MyDT\")")] DateTime MyDateTime { get; set; }
        IExampleNonexclusiveInterface MyInterfaceImplementer { get; set; }
        [IntroducedWithVersion(3)] string MyNewString { get; set; }

        NonLazinatorClass MyNonLazinatorChild { get; set; }
        bool MyNonLazinatorChild_Dirty { get; set; }
        [SetterAccessibility("internal")] decimal? MyNullableDecimal { get; }
        [RelativeOrder(-1)] // not necessary, but can confirm that it works
        [DerivationKeyword("virtual")] double? MyNullableDouble { get; set; }
        TimeSpan? MyNullableTimeSpan { get; set; }
        [EliminatedWithVersion(3)] string MyOldString { get; set; }
        [IncludeRefProperty]
        string MyString { get; set; }
        [Uncompressed]
        string MyStringUncompressed { get; set; }
        uint MyUint { get; set; }
        TestEnum MyTestEnum { get; set; }
        TestEnumByte? MyTestEnumByteNullable { get; set; }
        WInt WrappedInt { get; set; }
        [RelativeOrder(2)] // not necessary, but can confirm that it works
        [ExcludableChild] ExampleChild ExcludableChild { get; set; }
        [IncludableChild] ExampleChild IncludableChild { get; set; }
    }
}