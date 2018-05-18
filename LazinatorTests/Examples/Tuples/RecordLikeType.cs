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

    public struct DEBUG
    {
        public int Age { get; }
        public string Name { get; }

        public DEBUG(int age, string name)
        {
            this.Age = age;
            this.Name = name;
        }

        public DEBUG(int age, string name, string address)
        {
            throw new NotImplementedException(); // this constructor should not be called
        }

        public DEBUG(string age, string name)
        {
            throw new NotImplementedException(); // this constructor should not be called
        }

        public DEBUG(int age) : this(age, "NoName")
        {

        }

        public DEBUG(DEBUG other)
        {
            this = other;
        }
    }
}
