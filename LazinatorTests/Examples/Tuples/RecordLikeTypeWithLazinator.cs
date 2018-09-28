using System;

namespace LazinatorTests.Examples
{
    public readonly struct RecordLikeTypeWithLazinator
    {
        public int Age { get; }
        public string Name { get; }
        public Example Example { get; }

        public RecordLikeTypeWithLazinator(int age, string name, Example example)
        {
            Age = age;
            Name = name;
            Example = example;
        }
        
    }
}
