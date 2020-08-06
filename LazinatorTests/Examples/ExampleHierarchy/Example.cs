using Lazinator.Attributes;

namespace LazinatorTests.Examples
{
    [InsertCode(@"/* This is code added with an InsertCode attribute. */ 
        ")]
    [Implements(new string[] { "LazinatorObjectVersionUpgrade", "OnDirty", "OnDescendantIsDirty", "OnCompleteClone" })]
    public partial class Example : IExample
    {
        public Example()
        {

        }

        public void LazinatorObjectVersionUpgrade(int oldFormatVersion)
        {
            if (oldFormatVersion < 3 && LazinatorObjectVersion >= 3)
            {
                MyNewString = "NEW " + MyOldString;
                MyOldString = null;
            }
        }

        public bool _OnDirtyCalled = false;
        public bool _OnDescendantIsDirtyCalled = false;

        public void OnCompleteClone(Example other)
        {
            // could do something here
        }

        public void OnDirty()
        {
            _OnDirtyCalled = true;
        }

        public void OnDescendantIsDirty()
        {
            _OnDescendantIsDirtyCalled = true;
        }

        public void OnMyChild1Deserialized(ExampleChild myChild1)
        {
            // just for demonstration of attribute
        }

        public void OnMyChild1Accessed(ExampleChild myChild1)
        {
            // just for demonstration of attribute
        }
    }
}
