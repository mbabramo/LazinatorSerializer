namespace LazinatorFuzzTestGenerator.Interfaces
{
    public interface IObjectContents
    {
        ISupportedType TheType { get; }
        bool IsNull { get; }
        void Initialize(Random r);
        string CodeToInitializeValue { get; }
        string CodeToGetValue { get; }
        string CodeToTestValue(string containerName);
    }
}
