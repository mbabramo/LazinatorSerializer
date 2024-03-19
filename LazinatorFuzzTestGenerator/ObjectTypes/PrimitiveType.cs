using LazinatorFuzzTestGenerator.Interfaces;
using LazinatorFuzzTestGenerator.ObjectValues;
using static EnumFastToStringGenerated.PrimitiveEnumEnumExtensions;

namespace LazinatorFuzzTestGenerator.ObjectTypes
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
        public string UnannotatedTypeDeclaration() => PrimitiveEnum.ToDisplayFast();

        public IObjectContents GetRandomObjectContents(Random r, int? inverseProbabilityOfNull)
        {
            return new PrimitiveObjectContents(PrimitiveEnum, r, inverseProbabilityOfNull);
        }
    }
}
