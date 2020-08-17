using System;

namespace LazinatorTests.Examples
{
    public readonly struct RecordLikeStruct
    {
        public int Age { get; }
        public string Name { get; }

        public RecordLikeStruct(int age, string name)
        {
            this.Age = age;
            this.Name = name;
        }

        public RecordLikeStruct(int age, string name, string address)
        {
            throw new NotImplementedException(); // this constructor should not be called
        }

        public RecordLikeStruct(string age, string name)
        {
            throw new NotImplementedException(); // this constructor should not be called
        }

        public RecordLikeStruct(int age) : this(age, "NoName")
        {

        }

        public RecordLikeStruct(RecordLikeStruct other)
        {
            this = other;
        }
    }
}
