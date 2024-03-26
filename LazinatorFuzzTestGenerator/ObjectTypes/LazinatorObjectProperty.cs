using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LazinatorFuzzTestGenerator.Interfaces;

namespace LazinatorFuzzTestGenerator.ObjectTypes
{
    public record struct LazinatorObjectProperty(string propertyName, ISupportedType supportedType, bool nullable)
    {

        public string Declaration(bool nullableEnabledContext) => $"{supportedType.AnnotatedTypeDeclaration(nullable, nullableEnabledContext)} {propertyName} {{ get; set; }}";
    }
}
