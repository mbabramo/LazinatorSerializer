using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Specifies a condition in which a property will not be written or read, as well as optionally an initializer to execute on read if skipped.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SkipIfAttribute : Attribute
    {
        public string SkipCondition { get; set; }
        public string InitializeWhenSkipped { get; set; }

        public SkipIfAttribute(string skipCondition)
        {
            SkipCondition = skipCondition;
        }

        public SkipIfAttribute(string skipCondition, string initializeWhenSkipped)
        {
            SkipCondition = skipCondition;
            InitializeWhenSkipped = initializeWhenSkipped;
        }
    }
}
