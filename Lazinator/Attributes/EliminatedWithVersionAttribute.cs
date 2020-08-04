using System;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Indicates that a property has been eliminated as of a certain version and thus should not be serialized. The property is retained 
    /// in the interface and in the Lazinator object so that the information can still be read from earlier versions. A property can be
    /// deleted once the capability of reading the corresponding old versions is no longer needed.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EliminatedWithVersionAttribute : Attribute
    {
        public int Version;

        public EliminatedWithVersionAttribute(int version)
        {
            Version = version;
        }
    }
}
