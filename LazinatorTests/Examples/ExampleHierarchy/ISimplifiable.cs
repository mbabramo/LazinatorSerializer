using Lazinator.Attributes;

namespace LazinatorTests.Examples
{
    [Lazinator((int)ExampleUniqueIDs.IncludesSkippable)]
    interface ISimplifiable
    {
        [SkipIf("MyIntsAre3", "SetMyIntsTo3();")]
        int MyInt { get; set; }
        [SkipIf("MyIntsAre3")]
        int MyOtherInt { get; set; }
        bool ExampleHasDefaultValue { get; set; }
        [RelativeOrder(-1)]
        bool MyIntsAre3 { get; set; }
        [SkipIf("ExampleHasDefaultValue", "SetExampleToDefaultValue();")]
        Example Example { get; set; }
    }
}