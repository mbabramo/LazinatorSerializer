using Lazinator.Attributes;

namespace LazinatorTests.Examples
{
    [Lazinator((int)ExampleUniqueIDs.NonLazinatorInterchangeableStruct_LazinatorInterchange)]
    public interface INonLazinatorInterchangeStruct
    {
        string MyString { get; set; }
        int MyInt { get; set; }
    }
}
