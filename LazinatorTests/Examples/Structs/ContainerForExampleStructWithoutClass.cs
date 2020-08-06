namespace LazinatorTests.Examples.Structs
{
    public partial class ContainerForExampleStructWithoutClass : IContainerForExampleStructWithoutClass
    {
        public ContainerForExampleStructWithoutClass()
        {
        }

        public ExampleStructWithoutClass? ExampleNullableStruct { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }
}
