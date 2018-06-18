namespace LazinatorTests.Examples
{
    public class RecordLikeClass
    {
        public int Age { get; }
        public Example Example { get; }

        public RecordLikeClass(int age, Example example)
        {
            this.Age = age;
            this.Example = example;
        }
    }
}
