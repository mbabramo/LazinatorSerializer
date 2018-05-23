namespace LazinatorTests.Examples.Subclasses
{
    [Lazinator.Attributes.Lazinator((int)ExampleUniqueIDs.ClassWithSubclass)]
    public interface ISubclassWithinClass
    {
        string StringWithinSubclass { get; set; }
    }
}