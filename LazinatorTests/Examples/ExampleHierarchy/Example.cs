using Lazinator.Attributes;

namespace LazinatorTests.Examples
{
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
    }
}
