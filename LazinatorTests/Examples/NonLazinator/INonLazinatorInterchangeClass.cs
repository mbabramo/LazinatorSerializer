using Lazinator.Attributes;

namespace LazinatorTests.Examples
{
    [Lazinator((int)ExampleUniqueIDs.NonLazinatorInterchangeableClass_LazinatorInterchange)]
    public interface INonLazinatorInterchangeClass
    {
        string MyString { get; set; }
        int MyInt { get; set; }
    }
}
