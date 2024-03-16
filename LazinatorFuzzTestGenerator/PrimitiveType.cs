using static EnumFastToStringGenerated.PrimitiveEnumEnumExtensions;

namespace LazinatorFuzzTestGenerator
{
    public class PrimitiveType : ISupportedType
    {
        public PrimitiveEnum PrimitiveEnum { get; set; }

        public PrimitiveType(PrimitiveEnum primitiveEnum)
        {
            PrimitiveEnum = primitiveEnum;
        }

        public PrimitiveType(Random r)
        {
            PrimitiveEnum = GetRandomPrimitiveEnumType(r);
        }


        public bool UnannotatedIsNullable(bool nullableEnabledContext)
        {
            if (nullableEnabledContext)
                return false;
            if (PrimitiveEnum == PrimitiveEnum.String)
                return true;
            else
                return false;
        }

        private static PrimitiveEnum GetRandomPrimitiveEnumType(Random r)
        {
            return (PrimitiveEnum)r.Next(0, (int)PrimitiveEnum.Decimal);
        }
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
