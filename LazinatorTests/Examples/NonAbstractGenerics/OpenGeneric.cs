using Lazinator.Core;

namespace LazinatorTests.Examples.NonAbstractGenerics
{
    public partial class OpenGeneric<T> : IOpenGeneric<T> where T : ILazinator
    {
        // DEBUG; // Trying to move deserialization logic into DeserializeLazinator, but that causes a lot of failed tests. Why? May have to do with _MyT_Accessed, since that seems to change.
    }
}
