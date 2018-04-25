using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorCodeGen.Roslyn
{
    public static class AccessibilityConverter
    {
        public static string Convert(Microsoft.CodeAnalysis.Accessibility accessibility)
        {
            switch (accessibility)
            {
                case Microsoft.CodeAnalysis.Accessibility.Internal:
                    return "internal";
                case Microsoft.CodeAnalysis.Accessibility.NotApplicable:
                    return null;
                case Microsoft.CodeAnalysis.Accessibility.Private:
                    return "private";
                case Microsoft.CodeAnalysis.Accessibility.Protected:
                    return "protected";
                case Microsoft.CodeAnalysis.Accessibility.ProtectedOrInternal:
                    return "protected internal";
                case Microsoft.CodeAnalysis.Accessibility.Public:
                    return "public";
                default:
                    throw new NotSupportedException("Accessibility type not supported.");
            }
        }
    }
}
