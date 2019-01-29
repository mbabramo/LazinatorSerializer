namespace LazinatorTests.Examples.NonAbstractGenerics
{
    public partial class ClosedGeneric : OpenGeneric<ExampleChild>, IClosedGeneric
    {
        public ClosedGeneric()
        {
        }
    }
}
