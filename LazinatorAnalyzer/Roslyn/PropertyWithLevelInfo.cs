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
            IsDefinedLowerLevelButNotInInterface,
            IsDefinedInLowerLevelInterface
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
