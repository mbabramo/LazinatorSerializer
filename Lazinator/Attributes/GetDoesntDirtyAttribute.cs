using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Used to decorate a non-Lazinator property, when the item will never become dirty after the object is loaded.
    /// In this case, setting the property to a different value will lead to the containing object being marked as
    /// dirty, but merely loading the non-Lazinator property will not have that effect.
    /// Has no effect on primitive or Lazinator properties, for which merely accessing the object does not raise a
    /// dirtiness flag.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class GetDoesntDirtyAttribute : Attribute
    {
        public GetDoesntDirtyAttribute()
        {
        }
    }
}
