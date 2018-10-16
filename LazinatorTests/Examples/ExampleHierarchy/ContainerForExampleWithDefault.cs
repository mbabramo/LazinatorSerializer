namespace LazinatorTests.Examples
{
    public partial class ContainerForExampleWithDefault : IContainerForExampleWithDefault
    {
        public ContainerForExampleWithDefault()
        {
            Example = new Example()
            {
                MyChar = 'D'
            };
        }

    }
}
