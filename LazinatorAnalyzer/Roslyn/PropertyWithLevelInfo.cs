using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;

namespace LazinatorAnalyzer.Roslyn
{
    public class PropertyWithLevelInfo
    {
        public enum Level
        {
            IsDefinedThisLevel,
            IsDefinedConcretelyLowerLevel,
            IsDefinedAbstractlyLowerLevel
        }

        public IPropertySymbol Property;
        public Level LevelInfo;

        public PropertyWithLevelInfo(IPropertySymbol property, Level levelInfo)
        {
            this.Property = property;
            this.LevelInfo = levelInfo;
        }
    }
}
