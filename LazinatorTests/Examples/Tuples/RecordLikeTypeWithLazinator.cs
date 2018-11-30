using LazinatorTests.Examples.Structs;

namespace LazinatorTests.Examples
{
    public readonly struct RecordLikeTypeWithLazinator
    {
        public int Age { get; }
        public string Name { get; }
        public Example Example { get; }
        public ExampleStructWithoutClass ExampleStruct { get; }

        public RecordLikeTypeWithLazinator(int age, string name, Example example, ExampleStructWithoutClass exampleStruct)
        { 
            Age = age;
            Name = name;
            Example = example;
            ExampleStruct = exampleStruct;
        }
        
    }
}
