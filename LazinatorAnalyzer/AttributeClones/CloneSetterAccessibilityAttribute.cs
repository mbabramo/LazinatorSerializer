﻿using System;

namespace LazinatorAnalyzer.AttributeClones
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CloneSetterAccessibilityAttribute : Attribute
    {
        public string Choice { get; set; }

        public CloneSetterAccessibilityAttribute(string accessibility)
        {
            Choice = accessibility;
        }
    }
}
