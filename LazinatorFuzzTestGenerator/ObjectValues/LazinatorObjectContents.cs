using LazinatorFuzzTestGenerator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazinatorFuzzTestGenerator.ObjectValues
{
    public class LazinatorObjectContents : IObjectContents
    {
        public ISupportedType TheType { get; init; }
        public ILazinatorObjectType TheLazinatorObjectType => (ILazinatorObjectType)TheType;
        private List<object?>? PropertyValues { get; set; }

        public bool IsNull { get; set; }

        public LazinatorObjectContents(ISupportedType theType, Random r, int? inverseProbabilityOfNull)
        {
            TheType = theType;
            SetToRandom(r, inverseProbabilityOfNull);
        }

        private void SetToRandom(Random r, int? inverseProbabilityOfNull)
        {
            if (inverseProbabilityOfNull == null || r.Next((int)inverseProbabilityOfNull) != 0)
                SetToRandom(r);
            else
                PropertyValues = null;
        }

        public void SetToRandom(Random r)
        {
            var properties = TheLazinatorObjectType.Properties;
            PropertyValues = new List<object?>(properties.Count);
            for (int i = 0; i < properties.Count; i++)
            {
                var property = properties[i];
                var propertyType = property.supportedType;
                var isNullable = property.nullable;
                var value = propertyType.GetRandomObjectContents(r, property.nullable ? 3 : null);
                PropertyValues.Add(value);
            }
        }

        public string CodeToGetValue => throw new NotImplementedException();

        public string CodeToTestValue => throw new NotImplementedException();
    }
}
