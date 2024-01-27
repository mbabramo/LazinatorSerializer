using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorGenerator.AttributeClones
{
    /// <summary>
    /// Overrides the project settings for whether nullable context annotations will be enabled for a Lazinator class.
    /// </summary>
    public class CloneNullableContextSettingAttribute : Attribute
    {
        public bool Enabled { get; set; }

        public CloneNullableContextSettingAttribute(bool enabled)
        {
            Enabled = enabled;
        }
    }
}
