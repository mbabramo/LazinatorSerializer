namespace LazinatorFuzzTestGenerator.Interfaces
{
    public interface IObjectContents
    {
        ISupportedType TheType { get; }
        bool IsNull { get; }
        void Initialize(Random r);
        string CodeToReplicateContents { get; }
        string CodeToTestValue(string containerName);
    }
}
