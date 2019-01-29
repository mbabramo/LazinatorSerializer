namespace LazinatorTests.Examples.Subclasses
{
    public partial class ClassWithSubclass : IClassWithSubclass
    {
        public ClassWithSubclass()
        {
        }

        public partial class SubclassWithinClass : ISubclassWithinClass
        {
            public SubclassWithinClass()
            {
            }
        }
    }
}
