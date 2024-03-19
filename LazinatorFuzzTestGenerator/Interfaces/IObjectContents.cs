namespace LazinatorFuzzTestGenerator.Interfaces
{
    public interface IObjectContents
    {
        ISupportedType TheType { get; }
        bool IsNull { get; }
        void Initialize(Random r);
        string CodeToGetValue { get; }
        string CodeToTestValue(string containerName);
        string MutateAndReturnCodeForMutation(Random r, string containerName);
    }
}
