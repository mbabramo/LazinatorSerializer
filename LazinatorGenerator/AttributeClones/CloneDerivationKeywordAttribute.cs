﻿using System;

namespace LazinatorGenerator.AttributeClones
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CloneDerivationKeywordAttribute : Attribute
    {
        public string Choice { get; set; }

        public CloneDerivationKeywordAttribute(string derivationKeyword)
        {
            Choice = derivationKeyword;
        }
    }
}