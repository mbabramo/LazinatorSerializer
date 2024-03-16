﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazinatorFuzzTestGenerator
{
    public abstract class LazinatorObjectType : ISupportedType
    {
        public int UniqueID { get; init; }
        public abstract string DefinitionWord { get; }
        public string Name { get; init; }
        public List<LazinatorObjectProperty> Properties { get; init; }
        public abstract bool Inherits { get; }
        public abstract bool Inheritable { get; }
        public abstract bool Instantiable { get; }
        public abstract int ObjectDepth { get; }
        public abstract bool UnannotatedIsNullable(bool nullableEnabledContext);

        public LazinatorObjectType(int uniqueID, string name, List<LazinatorObjectProperty> properties)
        {
            UniqueID = uniqueID;
            Name = name;
            Properties = properties;
        }

        public string PropertyDeclarations(bool nullableEnabledContext)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var property in Properties)
            {
                sb.AppendLine(property.Declaration(nullableEnabledContext));
            }
            return sb.ToString();
        }

        public abstract string ILazinatorDeclaration(bool nullableEnabledContext);

        public abstract string UnannotatedTypeDeclaration();

        public abstract string ObjectDeclaration(bool nullableEnabledContext);
    }
}
