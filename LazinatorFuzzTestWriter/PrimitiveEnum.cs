using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnumFastToStringGenerated;
using Supernova.Enum;

namespace LazinatorFuzzTestWriter
{
    // private static readonly string[] SupportedAsPrimitives = new string[] {"bool", "byte", "sbyte", "char", "short", "ushort", "int", "uint", "long", "ulong", "string", "DateTime", "TimeSpan", "Guid", "float", "double", "decimal"};

    [EnumGenerator]
    public enum PrimitiveEnum
    {
        [Display(Name = "bool")]
        Bool,
        [Display(Name = "byte")]
        Byte,
        [Display(Name = "sbyte")]
        SByte,
        [Display(Name = "char")]
        Char,
        [Display(Name = "short")]
        Short,
        [Display(Name = "ushort")]
        UShort,
        [Display(Name = "int")]
        Int,
        [Display(Name = "uint")]
        UInt,
        [Display(Name = "long")]
        Long,
        [Display(Name = "ulong")]
        ULong,
        [Display(Name = "string")]
        String,
        [Display(Name = "DateTime")]
        DateTime,
        [Display(Name = "TimeSpan")]
        TimeSpan,
        [Display(Name = "Guid")]
        Guid,
        [Display(Name = "float")]
        Float,
        [Display(Name = "double")]
        Double,
        [Display(Name = "decimal")]
        Decimal
    }
}
