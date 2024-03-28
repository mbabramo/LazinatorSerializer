using LazinatorFuzzTestGenerator.Interfaces;
using LazinatorFuzzTestGenerator.ObjectTypes;

namespace LazinatorFuzzTestGenerator.ObjectValues
{
    public record struct PropertyWithContents(LazinatorObjectProperty property, LazinatorObjectContents parent, int indexInParent, IObjectContents? contents);
}
