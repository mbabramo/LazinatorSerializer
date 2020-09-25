namespace LazinatorTests.Examples.Tuples
{
    public partial class RecordLikeContainer : IRecordLikeContainer
    {
        public RecordLikeContainer()
        {
        }

        public NonLazinatorSubrecordWithConstructor MyNonLazinatorSubrecordWithConstructor { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public NonLazinatorSubrecordWithoutConstructor MyNonLazinatorSubrecordWithoutConstructor { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }
}
