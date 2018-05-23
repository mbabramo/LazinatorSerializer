namespace LazinatorTests.Examples.Subclasses
{
    [Lazinator.Attributes.Lazinator((int)ExampleUniqueIDs.SubclassWithinClass)]
    public interface ISubclassWithinClass
    {
        string StringWithinSubclass { get; set; }
    }
}