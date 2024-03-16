using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazinatorFuzzTestGenerator
{
    public record class LazinatorObjectTypeCollection(string namespaceString, bool nullableEnabledContext)
    {
        public List<LazinatorObjectType> ObjectTypes { get; set; } = new List<LazinatorObjectType>();
        public IEnumerable<LazinatorObjectType> InstantiableObjectTypes => ObjectTypes.Where(x => x.Instantiable);
        public IEnumerable<LazinatorClassType> InheritableClassTypes => ObjectTypes.Where(x => x.Inheritable).Select(x => (LazinatorClassType) x);
        int UniqueIDCounter = 10_000;
        int PropertyCounter = 0;

        public void GenerateObjectTypes(int numObjectTypes, int maximumDepth, int maxProperties, Random r)
        {
            for (int i = 0; i < numObjectTypes; i++)
            {
                int uniqueID = UniqueIDCounter++;
                int numProperties = r.Next(0, maxProperties);
                List<LazinatorObjectProperty> properties = new List<LazinatorObjectProperty>();
                for (int j = 0; j < numProperties; j++)
                {
                    properties.Add(GenerateObjectProperty(r));
                }
                if (r.Next(0, 2) == 0)
                {
                    LazinatorStructType structType = new LazinatorStructType(uniqueID, "S" + uniqueID, properties);
                    ObjectTypes.Add(structType);
                }
                else
                {
                    bool isAbstract = r.Next(0, 4) == 0;
                    bool isSealed = !isAbstract && r.Next(0, 3) == 0;
                    if (r.Next(0, 2) == 0 || !InheritableClassTypes.Any())
                    {
                        LazinatorClassType classType = new LazinatorClassType(uniqueID, "C" + uniqueID, isAbstract, isSealed, null, properties);
                        ObjectTypes.Add(classType);
                    }
                    else
                    {
                        LazinatorClassType parent = InheritableClassTypes.Where(x => x.ObjectDepth < maximumDepth).ToList()[r.Next(0, InheritableClassTypes.Count())];
                        LazinatorClassType classType = new LazinatorClassType(uniqueID, "C" + uniqueID, isAbstract, isSealed, parent, properties);
                        ObjectTypes.Add(classType);
                    }
                }
            }
        }


        public List<(string folder, string filename, string code)> GenerateSources()
        {
            List<(string folder, string filename, string code)> result = new List<(string folder, string filename, string code)>();
            foreach (LazinatorObjectType objectType in ObjectTypes)
            {
                result.Add((namespaceString, objectType.Name + ".cs", objectType.ObjectDeclaration(nullableEnabledContext)));
                result.Add((namespaceString, "I" + objectType.Name + ".cs", objectType.ILazinatorDeclaration(namespaceString, nullableEnabledContext)));
            }
            return result;
        }

        private LazinatorObjectProperty GenerateObjectProperty(Random r)
        {
            bool nullable = r.Next(0,2) == 1;

            if (r.Next(0, 2) == 0 || !InstantiableObjectTypes.Any())
            {
                var primitiveType = new PrimitiveType(r);
                if (primitiveType.UnannotatedIsNullable(nullableEnabledContext))
                    nullable = true; // can't have non-nullable string outside nullable enabled context
                LazinatorObjectProperty property = new LazinatorObjectProperty($"p{PropertyCounter++}", primitiveType, nullable);
                return property;
            }
            else
            {
                var instantiableChoices = InstantiableObjectTypes.ToList();
                LazinatorObjectType instantiableObject = instantiableChoices[r.Next(0, instantiableChoices.Count)];
                if (instantiableObject.UnannotatedIsNullable(nullableEnabledContext))
                    nullable = true; // can't have non-nullable class outside nullable enabled context
                LazinatorObjectProperty property = new LazinatorObjectProperty($"p{PropertyCounter++}", instantiableObject, nullable);
                return property;
            }   
        }
    }
}
