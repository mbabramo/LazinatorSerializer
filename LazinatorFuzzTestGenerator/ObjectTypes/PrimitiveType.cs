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

        public object? GetDefaultValueIfNotNullable()
        {
            switch (PrimitiveEnum)
            {
                case PrimitiveEnum.Bool:
                    return false;
                case PrimitiveEnum.Byte:
                    return 0;
                case PrimitiveEnum.SByte:
                    return 0;
                case PrimitiveEnum.Char:
                    return default(char);
                case PrimitiveEnum.Short:
                    return 0;
                case PrimitiveEnum.UShort:
                    return 0;
                case PrimitiveEnum.Int:
                    return 0;
                case PrimitiveEnum.UInt:
                    return 0;
                case PrimitiveEnum.Long:
                    return 0;
                case PrimitiveEnum.ULong:
                    return 0;
                case PrimitiveEnum.String:
                    return default(string);
                case PrimitiveEnum.DateTime:
                    return default(DateTime);
                case PrimitiveEnum.TimeSpan:
                    return default(TimeSpan);
                case PrimitiveEnum.Guid:
                    return default(Guid);
                case PrimitiveEnum.Float:
                    return 0;
                case PrimitiveEnum.Double:
                    return 0;
                case PrimitiveEnum.Decimal:
                    return 0;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
