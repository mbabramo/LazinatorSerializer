namespace LazinatorFuzzTestGenerator.Interfaces
{
    public interface IObjectContents
    {
        ISupportedType TheType { get; }
        string CodeToGetValue { get; }
        string CodeToTestValue { get; }
        bool IsNull { get; }
        void SetToRandom(Random r);
    }
}
