namespace LazinatorTests.Examples.Subclasses
{
    [Lazinator.Attributes.Lazinator((int)ExampleUniqueIDs.ClassWithLocalEnum)]
    public interface IClassWithLocalEnum
    {
        ClassWithLocalEnum.EnumWithinClass MyEnum { get; set; }
    }
}