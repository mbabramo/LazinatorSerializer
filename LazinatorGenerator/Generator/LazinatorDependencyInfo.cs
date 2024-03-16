using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace LazinatorGenerator.Generator
{
    public readonly struct LazinatorDependencyInfo
    {
        internal readonly ImmutableArray<string> Dependencies;
        internal readonly ImmutableArray<(string dependency, int hashCode)> DependenciesWithHashCodes;
        internal readonly bool HashCodesAvailable;
        internal readonly int HashCode;

        internal bool IsUninitialized => Dependencies.IsDefault & DependenciesWithHashCodes.IsDefault;

        public LazinatorDependencyInfo(ImmutableArray<string> dependencies)
        {
            this.Dependencies = dependencies;
            HashCodesAvailable = false;
            HashCode = dependencies.GetHashCode();
        }

        public LazinatorDependencyInfo(ImmutableArray<(string dependency, int hashCode)> dependenciesWithHashCodes)
        {
            DependenciesWithHashCodes = dependenciesWithHashCodes;
            HashCodesAvailable = true;
            HashCode = DependenciesWithHashCodes.GetHashCode();
        }

        public LazinatorDependencyInfo WithUpdatedHashCodes(Dictionary<string, int> typeNameToHashDictionary)
        {
            if (HashCodesAvailable)
            {
                var dependenciesWithHashCodes = ImmutableArray.Create(DependenciesWithHashCodes.Select(x => ((string dependency, int hashCode))(x.dependency, typeNameToHashDictionary.ContainsKey(x.dependency) ? typeNameToHashDictionary[x.dependency] : 0)).ToArray());
                return new LazinatorDependencyInfo(dependenciesWithHashCodes);
            }
            else
            {
                var dependenciesWithHashCodes = ImmutableArray.Create(Dependencies.Select(x => ((string dependency, int hashCode))(x, typeNameToHashDictionary.ContainsKey(x) ? typeNameToHashDictionary[x] : 0)).ToArray());
                return new LazinatorDependencyInfo(dependenciesWithHashCodes);
            }
        }

        public override int GetHashCode()
        {
            return HashCode;
        }
    }
}
