namespace LazinatorTests.Examples.NonAbstractGenerics
{
    public partial class ClosedGenericSealed : OpenGeneric<ExampleChild>, IClosedGenericSealed
    {
        public ClosedGenericSealed()
        {
        }
    }
}
