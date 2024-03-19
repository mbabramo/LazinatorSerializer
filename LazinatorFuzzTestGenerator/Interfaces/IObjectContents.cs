namespace LazinatorFuzzTestGenerator.Interfaces
{
    public interface IObjectContents
    {
        public ISupportedType TheType { get; }
        public object? Value { get; set; }
        public string CodeToGetValue { get; }
        public string CodeToTestValue { get; }
    }
}
