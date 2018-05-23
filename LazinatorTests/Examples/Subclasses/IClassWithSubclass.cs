namespace LazinatorTests.Examples.Subclasses
{
    [Lazinator.Attributes.Lazinator((int)ExampleUniqueIDs.ClassWithSubclass)]
    interface IClassWithSubclass
    {
        int IntWithinSuperclass { get; set; }
        ClassWithSubclass.SubclassWithinClass SubclassInstance1 { get; set; }
        ClassWithSubclass.SubclassWithinClass SubclassInstance2 { get; set; }
    }
}