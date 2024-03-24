namespace LazinatorFuzzTestGenerator.Interfaces
{
    public interface IObjectContents
    {
        ISupportedType TheType { get; }
        bool IsNull { get; }
        void Initialize(Random r);
        string CodeToGetValue { get; }
        string CodeToTestValue(string containerName);

        (string codeForMutation, (IObjectContents objectContents, string objectName)? additionalObject) MutateAndReturnCodeForMutation(Random r, string varName, bool canBeNull);
    }
}
