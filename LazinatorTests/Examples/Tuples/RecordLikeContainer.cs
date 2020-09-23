namespace LazinatorTests.Examples.Tuples
{
    public partial class RecordLikeContainer : IRecordLikeContainer
    {
        public RecordLikeContainer()
        {
        }

        public NonLazinatorRecordWithConstructor MyNonLazinatorRecordWithConstructor { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public NonLazinatorRecordWithoutConstructor MyNonLazinatorRecordWithoutConstructor { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }
}
