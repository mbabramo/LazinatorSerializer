using System;

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

        public RecordLikeType(int age, string name, string address)
        {
            throw new NotImplementedException(); // this constructor should not be called
        }

        public RecordLikeType(string age, string name)
        {
            throw new NotImplementedException(); // this constructor should not be called
        }

        public RecordLikeType(int age) : this(age, "NoName")
        {

        }

        public RecordLikeType(RecordLikeType other)
        {
            this = other;
        }
    }
}
