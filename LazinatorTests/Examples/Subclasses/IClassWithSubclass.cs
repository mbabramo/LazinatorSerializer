namespace LazinatorTests.Examples.Subclasses
{
    [Lazinator.Attributes.Lazinator((int)ExampleUniqueIDs.ClassWithSubclass)]
    interface IClassWithSubclass
    {
        int IntWithinSuperclass { get; set; }
    }
}