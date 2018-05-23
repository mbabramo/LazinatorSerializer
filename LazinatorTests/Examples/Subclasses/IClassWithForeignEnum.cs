namespace LazinatorTests.Examples.Subclasses
{
    [Lazinator.Attributes.Lazinator((int)ExampleUniqueIDs.ClassWithForeignEnum)]
    public interface IClassWithForeignEnum
    {
        ClassWithLocalEnum.EnumWithinClass MyEnum { get; set; }
    }
}