using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LazinatorCodeGen.Roslyn
{
    /// <summary>
    /// Determine string based on accessibility enum, or get accesibility enum from type declaration.
    /// </summary>
    public static class AccessibilityConverter
    {
        public static string Convert(Accessibility accessibility)
        {
            switch (accessibility)
            {
                case Accessibility.Internal:
                    return "internal";
                case Accessibility.NotApplicable:
                    return null;
                case Accessibility.Private:
                    return "private";
                case Accessibility.Protected:
                    return "protected";
                case Accessibility.ProtectedOrInternal:
                    return "protected internal";
                case Accessibility.Public:
                    return "public";
                default:
                    throw new NotSupportedException("Accessibility type not supported.");
            }
        }

        public static Accessibility GetAccessibility(this TypeDeclarationSyntax typeDeclaration)
        {
            bool isPrivate = typeDeclaration.Modifiers.Any(x => x.IsKind(SyntaxKind.PrivateKeyword));
            bool isInternal = typeDeclaration.Modifiers.Any(x => x.IsKind(SyntaxKind.InternalKeyword));
            bool isProtected = typeDeclaration.Modifiers.Any(x => x.IsKind(SyntaxKind.ProtectedKeyword));
            Accessibility typeAccessibility;
            if (isPrivate)
                typeAccessibility = Accessibility.Private;
            else if (isProtected)
            {
                if (isInternal)
                    typeAccessibility = Accessibility.ProtectedOrInternal;
                else
                    typeAccessibility = Accessibility.Protected;
            }
            else if (isInternal)
                typeAccessibility = Accessibility.Internal;
            else
                typeAccessibility = Accessibility.Public;

            return typeAccessibility;
        }
    }
}
