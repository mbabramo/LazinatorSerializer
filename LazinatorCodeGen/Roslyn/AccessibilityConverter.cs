using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorCodeGen.Roslyn
{
    public static class AccessibilityConverter
    {
        public static Lazinator.Attributes.Accessibility? Convert(Microsoft.CodeAnalysis.Accessibility accessibility)
        {
            switch (accessibility)
            {
                case Microsoft.CodeAnalysis.Accessibility.Internal:
                    return Lazinator.Attributes.Accessibility.Internal;
                case Microsoft.CodeAnalysis.Accessibility.NotApplicable:
                    return null;
                case Microsoft.CodeAnalysis.Accessibility.Private:
                    return Lazinator.Attributes.Accessibility.Private;
                case Microsoft.CodeAnalysis.Accessibility.Protected:
                    return Lazinator.Attributes.Accessibility.Protected;
                case Microsoft.CodeAnalysis.Accessibility.ProtectedOrInternal:
                    return Lazinator.Attributes.Accessibility.ProtectedInternal;
                case Microsoft.CodeAnalysis.Accessibility.Public:
                    return Lazinator.Attributes.Accessibility.Public;
                default:
                    throw new NotSupportedException("Accessibility type not supported.");
            }
        }
    }
}
