using static EnumFastToStringGenerated.PrimitiveEnumEnumExtensions;

namespace LazinatorFuzzTestWriter
{
    public class PrimitiveType : ISupportedType
    {
        public PrimitiveEnum PrimitiveEnum { get; set; }
        public string TypeDeclaration(bool nullable, bool nullableEnabledContext)
        {
            string declaration = PrimitiveEnum.ToDisplayFast();
            if (PrimitiveEnum == PrimitiveEnum.String && !nullable && !nullableEnabledContext)
                throw new NotImplementedException("String should not be non-nullable in a non-nullable-enabled-context");
                declaration = "string";
            if (nullable && (PrimitiveEnum != PrimitiveEnum.String || nullableEnabledContext))
                declaration += "?";
            return declaration;
        }
    }
}
