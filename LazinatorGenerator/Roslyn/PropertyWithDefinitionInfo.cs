using Microsoft.CodeAnalysis;

namespace LazinatorGenerator.Roslyn
{
    public class PropertyWithDefinitionInfo
    {
        public enum RelativeLevel
        {
            IsDefinedThisLevel,
            IsDefinedLowerLevelButNotInInterface,
            IsDefinedInLowerLevelInterface
        }

        public IPropertySymbol Property;
        public RelativeLevel WhereDefined;
        public int LevelsFromTop;
        public string PropertyAccessibility;
        public string DerivationKeyword;

        public override string ToString()
        {
            return $"{DerivationKeyword}{Property} ({WhereDefined})";
        }

        public PropertyWithDefinitionInfo(IPropertySymbol property, RelativeLevel levelInfo, int levelsFromTop)
        {
            this.Property = property;
            this.WhereDefined = levelInfo;
            this.LevelsFromTop = levelsFromTop;
        }

        public void SpecifyDerivationKeyword(string derivationKeyword)
        {
            this.DerivationKeyword = derivationKeyword;
        }

        public override bool Equals(object obj)
        {
            PropertyWithDefinitionInfo other = (PropertyWithDefinitionInfo) obj;
            if (other == null)
                return false;
            return SymbolEqualityComparer.Default.Equals(Property, other.Property) && WhereDefined == other.WhereDefined && LevelsFromTop == other.LevelsFromTop &&
                   DerivationKeyword == other.DerivationKeyword;
        }

        public override int GetHashCode()
        {
            return (Property, WhereDefined, DerivationKeyword, LevelsFromTop).GetHashCode();
        }
    }
}
