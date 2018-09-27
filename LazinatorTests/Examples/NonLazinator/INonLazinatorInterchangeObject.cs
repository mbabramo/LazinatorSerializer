using Lazinator.Attributes;

namespace LazinatorTests.Examples
{
    [Lazinator((int)ExampleUniqueIDs.NonLazinatorInterchangeableClass_LazinatorInterchange)]
    public interface INonLazinatorInterchangeObject
    {
        string MyString { get; set; }
        int MyInt { get; set; }
    }
}
