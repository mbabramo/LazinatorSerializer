using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;

namespace LazinatorAnalyzer.Roslyn
{
    public class PropertyWithDefinitionInfo
    {
        public enum Level
        {
            IsDefinedThisLevel,
            IsDefinedLowerLevelButNotInInterface,
            IsDefinedInLowerLevelInterface
        }

        public IPropertySymbol Property;
        public Level LevelInfo;
        public string DerivationKeyword;

        public PropertyWithDefinitionInfo(IPropertySymbol property, Level levelInfo)
        {
            this.Property = property;
            this.LevelInfo = levelInfo;
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
            return Property.Equals(other.Property) && LevelInfo == other.LevelInfo &&
                   DerivationKeyword == other.DerivationKeyword;
        }

        public override int GetHashCode()
        {
            return (Property, LevelInfo, DerivationKeyword).GetHashCode();
        }
    }
}
