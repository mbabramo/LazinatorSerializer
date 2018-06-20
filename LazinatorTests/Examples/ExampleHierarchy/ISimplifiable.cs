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
        [SetterAccessibility("private")]
        bool ExampleHasDefaultValue { get; }
        [SetterAccessibility("private")]
        char? Example2Char { get; }
        [SetterAccessibility("private")]
        bool Example3IsNull { get; }
        [RelativeOrder(-1)]
        [SetterAccessibility("private")]
        bool MyIntsAre3 { get; }
        [SkipIf("ExampleHasDefaultValue", "SetExampleToDefaultValue();")]
        Example Example { get; set; }
        [SkipIf("Example2Char != null", "Example2 = new Example() { MyChar = (char)Example2Char };")]
        [IntroducedWithVersion(4)]
        Example Example2 { get; set; }
        [SkipIf("Example3IsNull", "Example3 = null;")] // note: setting to null is required
        Example Example3 { get; set; }
    }
}