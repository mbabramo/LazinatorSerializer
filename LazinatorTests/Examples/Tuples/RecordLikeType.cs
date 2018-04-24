using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorTests.Examples
{
    public readonly struct RecordLikeType
    {
        public int Age { get; }
        public string Name { get; }

        public RecordLikeType(int age, string name)
        {
            this.Age = age;
            this.Name = name;
        }

        public RecordLikeType(RecordLikeType other)
        {
            this = other;
        }
    }
}
