using Lazinator.Attributes;

namespace LazinatorTests.Examples
{
    [Lazinator((int)ExampleUniqueIDs.IncludesSkippable)]
    interface ISimplifiable
    {
        [SkipIf("MyIntsAre3", "SetMyIntsTo3();")]
        int MyInt { get; set; }
        [SkipIf("MyIntsAre3", "// is set above")]
        int MyOtherInt { get; set; }
        bool ExampleHasDefaultValue { get; set; }
        char? Example2Char { get; set; }
        bool Example3IsNull { get; set; }
        [RelativeOrder(-1)]
        bool MyIntsAre3 { get; set; }
        [SkipIf("ExampleHasDefaultValue", "SetExampleToDefaultValue();")]
        Example Example { get; set; }
        [SkipIf("Example2Char != null", "Example2 = new Example() { MyChar = (char)Example2Char };")]
        [IntroducedWithVersion(4)]
        Example Example2 { get; set; }
        [SkipIf("Example3IsNull", "Example3 = null;")] // note: setting to null is required
        Example Example3 { get; set; }
    }
}