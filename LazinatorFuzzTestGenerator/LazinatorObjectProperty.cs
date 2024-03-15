using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazinatorFuzzTestGenerator
{
    public record struct LazinatorObjectProperty(string propertyName, ISupportedType supportedType, bool nullable)
    {
        public string Declaration(bool nullableEnabledContext) => $"public {supportedType.TypeDeclaration(nullable, nullableEnabledContext)} {propertyName} {{ get; set; }}";
    }
}
