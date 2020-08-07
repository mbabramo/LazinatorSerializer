using System.Collections.Generic;

namespace LazinatorTests.Examples
{
    public partial class ExampleContainerContainingClassesStructContainingClasses : IExampleContainerStructContainingClasses
    {
        public ExampleContainerContainingClassesStructContainingClasses()
        {
        }

        public List<ExampleStructContainingClasses?> MyListUnwrappedNullableExampleStruct { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }
}
