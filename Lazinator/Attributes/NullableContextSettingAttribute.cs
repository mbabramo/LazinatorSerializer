using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Attributes
{
    /// <summary>
    /// Overrides the project settings for whether nullable context annotations will be enabled for a Lazinator class.
    /// </summary>
    public class NullableContextSettingAttribute : Attribute
    {
        public bool Enabled { get; set; }

        public NullableContextSettingAttribute(bool enabled)
        {
            Enabled = enabled;
        }
    }
}
